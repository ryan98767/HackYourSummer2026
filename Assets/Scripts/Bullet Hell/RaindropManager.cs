using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 
/// </summary>
public class RaindropManager : MonoBehaviour
{
    /// <summary>
    /// A reference to the camera that's being used
    /// </summary>
    [SerializeField] private Camera usedCamera;

    /// <summary>
    /// The number of how many raindrops should fall on the screen per second
    /// </summary>
    [SerializeField] private float raindropSpawnRate;

    /// <summary>
    /// How fast the raindrops fall in pixels per second
    /// </summary>
    [SerializeField] private float raindropSpeed;

    /// <summary>
    /// The direction the next iteration of raindrops will fall at
    /// </summary>
    [SerializeField] private Vector2 raindropDirection;

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
    /// 
    /// </summary>
    private float timePassed;

    /// <summary>
    /// Initializes the fields that haven't been properly populated yet
    /// </summary>
    public void Start()
    {
        this.timePassed = 0f;
        this.inactiveRaindrops = new Queue<BulletHellRaindrop>();
        this.activeRaindrops = new List<BulletHellRaindrop>();
    }


    /// <summary>
    /// Updates the time between frames and the active raindrops
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    public void Tick(float dt)
    {
        IntegrateTime(dt);
        UpdateRaindrops(dt);
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
    private void IntegrateTime(float dt)
    {
        this.timePassed += dt;

        float spawnInterval = 1f / this.raindropSpawnRate;

        // Uses a while-loop so that multiple raindrops can spawn in a singular frame
        while (this.timePassed >= spawnInterval)
        {
            SpawnRaindrop();
            this.timePassed -= spawnInterval;
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
        }

        // Get the spawn position, and then properly initialize the object
        Vector3 spawnPosition = GetSpawnPosition();

        upcomingRaindrop.Initalize(spawnPosition, this.raindropDirection.normalized,
            this.raindropSpeed, this);

        upcomingRaindrop.gameObject.SetActive(true);
        this.activeRaindrops.Add(upcomingRaindrop);
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

        float randomXSpawn = Random.Range(-SpawningMargin, 1f + SpawningMargin);
        float ySpawn = SpawningYCoordinate + SpawningMargin;

        // Convert a new viewport into world coords
        Vector3 convertedViewport = new Vector3(randomXSpawn, ySpawn, 0f);
        Vector3 worldPosition = this.usedCamera.ViewportToWorldPoint(convertedViewport);

        // Ensure that the z-cord is nonexistent
        worldPosition.z = 0f;

        return worldPosition;
    }
}
