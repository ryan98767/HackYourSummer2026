using UnityEngine;

/// <summary>
/// Controls the behavior between different stages of the microgame
/// </summary>
public class BulletHellStateMachine : MonoBehaviour
{
    /// <summary>
    /// The different game states that exist in this microgame
    /// </summary>
    /// <remarks>This is made primarily so </remarks>
    public enum GameStateType
    {
        PREGAME,
        INGAME,
        POSTGAME,
        PAUSE,
    }


    [Header("UIManagers")]
    /// <summary>
    /// A referebce to the tutorial's UI Manager
    /// </summary>
    [SerializeField] private BulletHellTutorialUIManager tutorialUIManager;

    [SerializeField] private BulletHellInGameUIManager inGameUIManager;


    [Header("Exit Areas")]
    /// <summary>
    /// The area to which the player exits the tutorial 
    /// </summary>
    [SerializeField] private WalkableArea tutorialExitArea;

    [Header("Prefabs")]
    /// <summary>
    /// The raindrop that's being shown to the player
    /// </summary>
    [SerializeField] private BulletHellRaindrop showcaseRaindrop;


    [Header("Managers")]
    /// <summary>
    /// The manager meant to run the entireity of the game
    /// </summary>
    [SerializeField] private BulletHellGameManager gameManager;


    [Header("Necessary Nodes")]
    [SerializeField] private GameObject player;


    [Header("Initial State Selection")]
    /// <summary>
    /// The state that the game will initially start in.
    /// </summary>
    /// <remarks>NOTE: PAUSE CANNOT BE THE INITIAL STATE!</remarks>
    [SerializeField] GameStateType initialGameState;

    /// <summary>
    /// The current game state that's running
    /// </summary>
    private GameState currentState;

    /// <summary>
    /// A reference to the prestate of the bullet hell
    /// </summary>
    private BulletHellPreState preState;

    private BulletHellInGameState inGameState;

    /// <summary>
    /// Initializes all of the states and begins the initial state.
    /// </summary>
    /// <remarks>For the most part, the initial state should always be the pre-state, thus why I 
    /// have it hardcoded. However, maybe for debugging purposes, I can add a serialized field
    /// that allows you to select.</remarks>
    public void Start()
    {
        this.preState = new BulletHellPreState(this.showcaseRaindrop, this.tutorialExitArea, 
            this.tutorialUIManager);
        this.preState.RequestedTransition += SwitchToState;

        this.inGameState = new BulletHellInGameState(this.player, this.gameManager, 
            this.inGameUIManager);
        this.inGameState.RequestedTransition += SwitchToState;

        this.SwitchToState(this.initialGameState);
    }

    /// <summary>
    /// Updates the game based on the current state
    /// </summary>
    public void Update()
    {
        this.currentState?.Tick(Time.deltaTime);
    }

    /// <summary>
    /// Switches to a state
    /// </summary>
    /// <param name="whichState">whatever state that's wanting to be switched into</param>
    private void SwitchToState(GameStateType whichState)
    {
        // Ensure that a nullexception can't happen here
        this.currentState?.Exit();

        this.currentState = FindGameState(whichState);
        this.currentState.Enter();
    }

    /// <summary>
    /// Finds a reference to a particular game state based on the GameStateType enum
    /// </summary>
    /// <param name="onWhatState">the particular moment in the game that'd like to be switched
    /// into</param>
    /// <returns>the reference to the desired state</returns>
    private GameState FindGameState(GameStateType onWhatState)
    {
        switch (onWhatState)
        {
            case GameStateType.PREGAME:
                return this.preState;
            case GameStateType.INGAME:
                return this.inGameState;
            default:
                return null;
        }
    }
}
