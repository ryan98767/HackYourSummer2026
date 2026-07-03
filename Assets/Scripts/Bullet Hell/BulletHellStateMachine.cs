using UnityEngine;

/// <summary>
/// Controls the behavior between different stages of the microgame
/// </summary>
public class BulletHellStateMachine : MonoBehaviour
{
    /// <summary>
    /// A referebce to the tutorial's UI Manager
    /// </summary>
    [SerializeField] private TutorialUIManager tutorialUIManager;

    /// <summary>
    /// The area to which the player exits the tutorial 
    /// </summary>
    [SerializeField] private WalkableArea tutorialExitArea;

    /// <summary>
    /// The raindrop that's being shown to the player
    /// </summary>
    [SerializeField] private BulletHellRaindrop showcaseRaindrop;

    /// <summary>
    /// The current game state that's running
    /// </summary>
    private GameState currentState;

    /// <summary>
    /// A reference to the prestate of the bullet hell
    /// </summary>
    private BulletHellPreState preState;

    /// <summary>
    /// Initializes all of the states and begins the initial state.
    /// <remarks>For the most part, the initial state should always be the pre-state, thus why I 
    /// have it hardcoded. However, maybe for debugging purposes, I can add a serialized field
    /// that allows you to select.</remarks>
    /// </summary>
    public void Start()
    {
        this.preState = new BulletHellPreState(this.showcaseRaindrop, this.tutorialExitArea, 
            this.tutorialUIManager);

        SwitchToState(this.preState);
    }

    /// <summary>
    /// Updates the game based on the current state
    /// </summary>
    public void LateUpdate()
    {
        this.currentState?.Tick(Time.deltaTime);
    }

    /// <summary>
    /// Switches to a state
    /// </summary>
    /// <param name="whichState">whatever state that's wanting to be switched into</param>
    private void SwitchToState(GameState whichState)
    {
        // Ensure that a nullexception can't happen here
        this.currentState?.Exit();
        this.currentState = whichState;
        this.currentState.Enter();
    }
}
