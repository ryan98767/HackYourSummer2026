using UnityEngine;

/// <summary>
/// Controls everything involving the post-game state of the bullet hell microgame
/// </summary>
public class BulletHellPostGameState : GameState
{
    /// <summary>
    /// A reference to the UI manager being used
    /// </summary>
    private BulletHellPostGameUIManager uiManager;

    /// <summary>
    /// The final score the player got
    /// </summary>
    private float finalScore;


    /// <summary>
    /// Populates the UI manager so it can begin working with the score
    /// </summary>
    /// <param name="uiManager"></param>
    public BulletHellPostGameState(BulletHellPostGameUIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    /// <summary>
    /// The actions to occur once the player dies
    /// </summary>
    /// <param name="finalScore">the final score the player has before dying</param>
    public void OnPlayerDeath(float finalScore)
    {
        this.finalScore = finalScore;
    }

    /// <summary>
    /// Feeds the final score to the UI manager
    /// </summary>
    public override void Enter()
    {
        this.uiManager.Initialize(this.finalScore);
        this.uiManager.Show();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick(float dt)
    {
        this.uiManager.Tick(dt);
    }
}
