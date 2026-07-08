using UnityEngine;

public class BulletHellInGameState : GameState
{
    /// <summary>
    /// A reference to the game manager that's being used for the game
    /// </summary>
    private BulletHellGameManager gameManager;

    /// <summary>
    /// A reference to the player 
    /// </summary>
    private GameObject player;

    /// <summary>
    /// A reference to the in-game UI manager
    /// </summary>
    private BulletHellInGameUIManager uiManager;


    /// <summary>
    /// Prepares the state for when it's actually going to be used
    /// </summary>
    /// <param name="player">a reference to the player GO</param>
    /// <param name="gameManager">a reference to the game manager that'll be used for the active 
    /// session</param>
    /// <param name="uiManager">a refernce to the UI manager used during the game session</param>
    public BulletHellInGameState(GameObject player, BulletHellGameManager gameManager, BulletHellInGameUIManager uiManager)
    {
        this.player = player;
        this.gameManager = gameManager;
        this.uiManager = uiManager;
    }


    /// <summary>
    /// Adds the starter stats to the main game before starting
    /// </summary>
    public override void Enter()
    {
        // Initialize the game manager and its events
        this.gameManager.Initialize();
        this.gameManager.PlayerDied += OnPlayerDeath;
        this.gameManager.PlayerTookDamage += OnPlayerDamage;
        this.gameManager.ScoreUpdated += OnScoreUpdated;

        // Initialize the UI and have it appear
        this.uiManager.Initialize(3);
        this.uiManager.Show();

        // Finally, start the game
        this.gameManager.StartGame();
    }

    public override void Exit()
    {
        this.gameManager.gameObject.SetActive(false);
        this.uiManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the majority of the game
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    public override void Tick(float dt)
    {
        this.gameManager.Tick(dt);
        this.uiManager.Tick(dt);
    }

    /// <summary>
    /// The actions that should occur once the player has taken damage
    /// </summary>
    /// <remarks>I really should've made an event bus... The stack would be the Raindrop, then this
    /// manager, then the GameManager, then the InGameState, then the UIState</remarks>
    /// <see cref="BulletHellRaindrop.OnTriggerEnter2D(Collider2D)"/>
    /// <seealso cref="RaindropManager.OnHitPlayer"/>
    private void OnPlayerDamage()
    {
        this.uiManager.UpdateHearts();
    }

    /// <summary>
    /// The events that should occur once the player dies
    /// </summary>
    /// <param name="finalScore">the final score that the player got</param>
    private void OnPlayerDeath(float finalScore)
    {
        RequestTransition(BulletHellStateMachine.GameStateType.POSTGAME);
    }

    /// <summary>
    /// The actions to take place one the score is in need of updating UI-wise
    /// </summary>
    /// <param name="toWhat">the value of which to make the score</param>
    private void OnScoreUpdated(float toWhat)
    {
        this.uiManager.UpdateScore(toWhat);
    }
}
