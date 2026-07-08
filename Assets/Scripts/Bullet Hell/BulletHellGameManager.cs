using System;
using UnityEngine;

/// <summary>
/// Manages the raindrops and the relationsip between the raindrop manager and the main state
/// machine
/// </summary>
public class BulletHellGameManager : MonoBehaviour
{
    /// <summary>
    /// Invoked once the player has died
    /// </summary>
    /// <remarks>This should only be invoked after all animations have been played
    /// and the game has completely ceased</remarks>
    public event Action<float> PlayerDied;

    /// <summary>
    /// Invoked once the player has taken damage
    /// </summary>
    public event Action PlayerTookDamage;

    /// <summary>
    /// Invoked once the score has been updated
    /// </summary>
    public event Action<float> ScoreUpdated;

    /// <summary>
    /// A reference to the raindrop manager that's being used for the active game session
    /// </summary>
    [SerializeField] private RaindropManager raindropManager;

    /// <summary>
    /// A reference to the player GameObject
    /// </summary>
    [SerializeField] private GameObject player;

    /// <summary>
    /// The score that the player has at any given moment
    /// </summary>
    private float currentScore;

    /// <summary>
    /// The accumulator that holds how long it's been since score has been given
    /// </summary>
    private float scoreTimer;

    /// <summary>
    /// The number of lives the player has
    /// </summary>
    private int currentLives;


    /// <summary>
    /// The actions that occur before a game starts
    /// </summary>
    public void Initialize()
    {
        this.currentScore = 0f;
        this.scoreTimer = 0f;
        this.currentLives = 3;

        this.raindropManager.PlayerHit += OnPlayerHit;
    }

    /// <summary>
    /// Kicks off a game
    /// </summary>
    public void StartGame()
    {
        // Allows the rendering of everything to occur here
        this.gameObject.SetActive(true);

        this.raindropManager.Initialize();
        this.raindropManager.gameObject.SetActive(true);
    }

    /// <summary>
    /// The actions that should occur every frame during the game
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    public void Tick(float dt)
    {
        // Update all of the raindrops
        this.raindropManager.Tick(dt);

        // Check for lives
        if (this.currentLives <= 0)
        {
            // Turn off the managers first
            this.raindropManager.EndGame();

            this.PlayerDied.Invoke(this.currentScore);
        }

        // Update the score
        UpdateScoreTimer(dt);
    }

    /// <summary>
    /// The actions to be taken once the player is hit
    /// </summary>
    /// <remarks>I really should've made an event bus... The stack would be the Raindrop, then this
    /// manager, then the GameManager, then the InGameState, then the UIState</remarks>
    /// <see cref="BulletHellRaindrop.OnTriggerEnter2D(Collider2D)"/>
    /// <seealso cref="RaindropManager.OnHitPlayer"/>
    /// <seealso cref="BulletHellInGameState.OnPlayerDamage"/>
    private void OnPlayerHit()
    {
        this.currentLives--;
        this.PlayerTookDamage.Invoke();
    }

    /// <summary>
    /// Updates score, when necessary
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    private void UpdateScoreTimer(float dt)
    {
        this.scoreTimer += dt;

        while (this.scoreTimer >= 1f)
        {
            this.currentScore += 10f;
            this.scoreTimer -= 1f;

            this.ScoreUpdated.Invoke(this.currentScore);
        }
    }
}
