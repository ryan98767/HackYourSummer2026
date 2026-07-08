using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class RaindropManager : MonoBehaviour
{
    public event Action PlayerHit;

    [Header("Init Game Values")]
    /// <summary>
    /// The number of how many raindrops should fall on the screen per second
    /// </summary>
    [SerializeField] private float raindropSpawnRate;

    /// <summary>
    /// How often the direction and speed change, in seconds
    /// </summary>
    [SerializeField] private float difficultyInterval;

    /// <summary>
    /// How fast the raindrops fall in pixels per second
    /// </summary>
    [SerializeField] private float raindropSpeed;

    /// <summary>
    /// The direction the next iteration of raindrops will fall at
    /// </summary>
    [SerializeField] private Vector2 raindropDirection;

    [Header("GameObject references")]
    /// <summary>
    /// A reference to the camera that's being used
    /// </summary>
    [SerializeField] private Camera usedCamera;

    [SerializeField] private BulletHellRaindrop raindropPrefab;

    /// <summary>
    /// A reference to the raindrops that aren't in use and waiting to be pooled
    /// </summary>
    private Queue<BulletHellRaindrop> inactiveRaindrops;
    
    /// <summary>
    /// A reference to all of the raindrops that are currently in use
    /// </summary>
    private List<BulletHellRaindrop> activeRaindrops;

    /// <summary>
    /// The base amount of speed the raindrops will increase by
    /// </summary>
    private const int RaindropBaseStepIncrease = 1;

    /// <summary>
    /// How long it's been since a newly spawned raindrop spawned
    /// </summary>
    private float spawnInterval;

    /// <summary>
    /// How long it's been since a new change in direction to the raindrops
    /// </summary>
    private float raindropChangeInterval;

    /// <summary>
    /// Whether or not the game is active
    /// </summary>
    private bool isGameOn;


    /// <summary>
    /// Sets up the initial values for each major field prior to the start of the game
    /// </summary>
    public void Initialize()
    {
        this.spawnInterval = 0f;
        this.raindropChangeInterval = 0f;
        this.inactiveRaindrops = new Queue<BulletHellRaindrop>();
        this.activeRaindrops = new List<BulletHellRaindrop>();
        this.isGameOn = true;
    }


    /// <summary>
    /// Updates the time between frames and the active raindrops
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    public void Tick(float dt)
    {
        if (this.isGameOn)
        {
            IntegrateSpawningTime(dt);
            IntegrateDirectionChangeTime(dt);
            UpdateRaindrops(dt);
        }
    }

    /// <summary>
    /// Removes each reference to the raindrops and current gameobject while also stopping all 
    /// future tick references
    /// </summary>
    public void EndGame()
    {
        // First, set all the active raindrops to be inactive
        for (int i = this.activeRaindrops.Count - 1; i >= 0; i--)
        {
            BulletHellRaindrop currentRaindrop = this.activeRaindrops[i];
            this.activeRaindrops.RemoveAt(i);

            this.inactiveRaindrops.Enqueue(currentRaindrop);
        }

        // Next, destroy each raindrop GO and clear it from the queue
        while (this.inactiveRaindrops.TryDequeue(out BulletHellRaindrop dequeuedRaindrop))
        {
            Destroy(dequeuedRaindrop.gameObject);
        }

        this.inactiveRaindrops.Clear();

        this.isGameOn = false;
    }

    /// <summary>
    /// The actions that should occur once a raindrop needs to be despawned
    /// </summary>
    /// <param name="whichOne">the raindrop in question that needs to be despawned</param>
    private void Despawn(BulletHellRaindrop whichOne)
    {
        // Disables rendering and any other callbacks that may be made to the unneeded raindrop
        whichOne.gameObject.SetActive(false);

        this.inactiveRaindrops.Enqueue(whichOne);
    }

    /// <summary>
    /// Uses time in the minigame to determine whether to spawn another raindrop
    /// </summary>
    /// <param name="dt">a reference to the change in time</param>
    private void IntegrateSpawningTime(float dt)
    {
        this.spawnInterval += dt;

        float spawnInterval = 1f / this.raindropSpawnRate;

        // Uses a while-loop so that multiple raindrops can spawn in a singular frame
        while (this.spawnInterval >= spawnInterval)
        {
            SpawnRaindrop();
            this.spawnInterval -= spawnInterval;
        }
    }

    /// <summary>
    /// Uses time in the minigame to determine whether to change the direction and speed of 
    /// the raindrops
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    private void IntegrateDirectionChangeTime(float dt)
    {
        this.raindropChangeInterval += dt;

        while (this.raindropChangeInterval >= this.difficultyInterval)
        {
            UpdateRaindropDifficulty();
            this.raindropChangeInterval -= raindropChangeInterval;
        }
    }

    /// <summary>
    /// Spawns a raindrop, based on the pool, or a brand new one that's needed to be made
    /// </summary>
    private void SpawnRaindrop()
    {
        BulletHellRaindrop upcomingRaindrop;

        // First, see whether or not a raindrop is being unused
        if (this.inactiveRaindrops.Count > 0)
        {
            upcomingRaindrop = this.inactiveRaindrops.Dequeue();
        }
        // No? We gotta create a new one then
        else
        {
            upcomingRaindrop = Instantiate(this.raindropPrefab);
            upcomingRaindrop.HitPlayer += OnHitPlayer;
        }

        // Get the spawn position, and then properly initialize the object
        Vector3 spawnPosition = GetSpawnPosition();

        upcomingRaindrop.Initalize(spawnPosition, this.raindropDirection.normalized,
            this.raindropSpeed, this);

        upcomingRaindrop.gameObject.SetActive(true);
        this.activeRaindrops.Add(upcomingRaindrop);
    }

    /// <summary>
    /// The actions to happen once the player is hit
    /// </summary>
    /// <remarks>I really should've made an event bus... The stack would be the Raindrop, then this
    /// manager, then the GameManager, then the InGameState, then the UIState</remarks>
    /// <see cref="BulletHellGameManager"/>
    private void OnHitPlayer()
    {
        this.PlayerHit.Invoke();
    }

    /// <summary>
    /// Updates each of the raindrops that are being actively used and ensures that they aren't
    /// needed to be despawned
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    private void UpdateRaindrops(float dt)
    {
        for (int i = this.activeRaindrops.Count - 1; i >= 0; i--)
        {
            /*
             * Uses a reference of the raindrop instead of just constantly repeating it
             * (yeah, i know I've been doing alot of this. my fault)
             */
            BulletHellRaindrop activeRaindrop = this.activeRaindrops[i];
            activeRaindrop.Tick(dt, this.usedCamera);

            if (activeRaindrop.NeedsDespawn)
            {
                Despawn(activeRaindrop);
                this.activeRaindrops.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Uses the bounds of the camera to generate a random spawn point for a raindrop that's
    /// slightly outside the camera
    /// </summary>
    /// <returns>a random position outside the camera for the raindrop to spawn</returns>
    private Vector3 GetSpawnPosition()
    {
        // How far away from the edge of the camera we'd like to spawn the raindrop
        const float SpawningMargin = 0.1f;
        
        /*
         * The base postiion each raindrop will spawn in (we could make it so that it comes from a
         * different place in a different form, but for now it doesn't make much sense)
         */
        const float SpawningYCoordinate = 1f;

        float randomXSpawn = UnityEngine.Random.Range(-SpawningMargin, 1f + SpawningMargin);
        float ySpawn = SpawningYCoordinate + SpawningMargin;

        // Convert a new viewport into world coords
        Vector3 convertedViewport = new Vector3(randomXSpawn, ySpawn, 0f);
        Vector3 worldPosition = this.usedCamera.ViewportToWorldPoint(convertedViewport);

        // Ensure that the z-cord is nonexistent
        worldPosition.z = 0f;

        return worldPosition;
    }

    /// <summary>
    /// Updates the speed and direction of raindrops
    /// </summary>
    private void UpdateRaindropDifficulty()
    {
        // Firstly, increase the speed.
        // The speed increase is mostly random, with the step modifier acting as the median
        float increasedSpeedRate = UnityEngine.Random.Range(RaindropBaseStepIncrease / 2, 
            RaindropBaseStepIncrease * 2 + 1);

        this.raindropSpeed += increasedSpeedRate;

        // Next, determine a new direction
        // The direction is based on a random angle from 225 (the default), to 315
        float newRaindropDirection = UnityEngine.Random.Range(225, 315 + 1) * Mathf.Deg2Rad;

        this.raindropDirection = new Vector2(
            Mathf.Cos(newRaindropDirection),
            Mathf.Sin(newRaindropDirection)
            ).normalized;

        /*
         * Finally, increase the number of raindrops that'll fall while also 
         * decreasing the interval of difficulity to have at least *some*
         * level of balancing
         */
        this.raindropSpawnRate++;
        this.difficultyInterval += 0.5f;
    }
}
