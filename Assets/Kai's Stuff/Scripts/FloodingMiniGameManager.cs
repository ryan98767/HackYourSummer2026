using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the win/loss logic for the flooding minigame
/// This script checks whether the player escapes or gets caught by rising water
/// </summary>
public class FloodingMiniGameManager : MonoBehaviour
{
    public static FloodingMiniGameManager Instance;

    [Header("Game Settings")]
    public float miniGameTime = 30f;
    private float currentTime;

    [Header("Game State")]
    public bool gameIsActive;
    public bool gameHasEnded;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartMiniGame();
    }

    
    private void Update()
    {
        if (!gameIsActive || gameHasEnded)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            LoseMiniGame();
        }
    }

    /// <summary>
    /// Starts the minigame and resets the timer
    /// </summary>
    public void StartMiniGame()
    {
        currentTime = miniGameTime;
        gameIsActive = true;
        gameHasEnded = false;

        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.SetCurrentMiniGame("Flooding Building");
        }
    }

    /// <summary>
    /// Called when the player reaches the exit
    /// </summary>
    public void WinMiniGame()
    {
        if (gameHasEnded)
        {
            return;
        }

        gameHasEnded = true;
        gameIsActive = false;

        Debug.Log("Player escaped the flooding building!");

        if (PlayerDataManager.Instance != null)
        {
            //Bonus points for reaching the exit
            PlayerDataManager.Instance.AddScore(100);

            //Records the minigame as completed
            //passing 0 prevents double-counting score here
            PlayerDataManager.Instance.MiniGameCompleted(0);
        }

        //Sends player back to the main gameplay scene
        if (ProgramSceneManager.Instance != null)
        {
            ProgramSceneManager.Instance.LoadGameplay();
        }
    }

    public void LoseMiniGame()
    {
        if (gameHasEnded)
        {
            return;
        }

        gameHasEnded = true;
        gameIsActive = false;

        Debug.Log("Player lost the flooding minigame.");

        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.MiniGameFailed();
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
