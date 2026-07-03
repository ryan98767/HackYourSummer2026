using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Dictates all the actions that need to take place before the minigame begins
/// </summary>
public class BulletHellPreState : GameState
{
    /// <summary>
    /// A reference to the raindrop that's being shown in the tutorial
    /// </summary>
    private BulletHellRaindrop showcaseRaindrop;

    /// <summary>
    /// A reference to the exit area for the player to leave the tutorial
    /// </summary>
    private WalkableArea exitArea;

    /// <summary>
    /// A reference to the tutorial's UI manager
    /// </summary>
    private TutorialUIManager tutorialUIManager;


    /// <summary>
    /// Considers all the necessary fields to creating a working tutorial for the 
    /// bullet hell
    /// </summary>
    /// <param name="showcaseRaindrop">a reference to the raindrop that shows the player what 
    /// they're avoiding</param>
    /// <param name="exitArea">the area where the prestate will end</param>
    /// <param name="tutorialUIManager">The tutorial manager that assists in the creation
    /// and management of the tutorial's UI</param>
    public BulletHellPreState(BulletHellRaindrop showcaseRaindrop, WalkableArea exitArea,
        TutorialUIManager tutorialUIManager)
    {
        this.showcaseRaindrop = showcaseRaindrop;
        this.exitArea = exitArea;
        this.tutorialUIManager = tutorialUIManager;
        this.exitArea.OnTargetEntered += Exit;
    }

    /// <summary>
    /// Shows the tutorial label while also providing the necessary references to the 
    /// following label for it to follow the player
    /// </summary>
    public override void Enter()
    {
        this.tutorialUIManager.Show();
        this.tutorialUIManager.InitializeLabels();
    }

    /// <summary>
    /// Hides the UI and any other unnecessary objects and begins the game
    /// </summary>
    public override void Exit()
    {
        this.showcaseRaindrop.gameObject.SetActive(false);
        this.tutorialUIManager.Hide();
    }

    /// <summary>
    /// Allows the label to follow the player.
    /// <remarks>Delta time wouldn't be needed to follow the player here, which is why I didn't include it</remarks>
    /// </summary>
    public override void Tick(float dt)
    {
        this.tutorialUIManager.Tick();
    }
}
