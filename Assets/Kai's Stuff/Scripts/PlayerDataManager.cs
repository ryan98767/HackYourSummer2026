using UnityEngine;

/// <summary>
/// Stores player data that needs to be shared across scenes and minigames
/// This script should stay alive while the game is running
/// </summary>
public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    [Header("Player Stats")]
    public int Score;
    public int Lives;
    public int miniGamesCompleted;
    public int miniGamesFailed;

    [Header("Mini Game Data")]
    public string CurrentMiniGameName;
    public float TotalPlayTime;

    private void Awake()
    {
        //Makes sure only one PlayerDataManager exists
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //Keeps player data when switching scenes
        DontDestroyOnLoad(gameObject);
    }
    
    void Update()
    {
        //Only tracks time while the game is not paused.
        if (Time.timeScale > 0)
        {
            TotalPlayTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Resets all player data for a new game
    /// Call this when the player starts from the main menu
    /// </summary>
    public void ResetPlayerData()
    {
        Score = 0;
        Lives = 3;
        miniGamesCompleted = 0;
        miniGamesFailed = 0;
        CurrentMiniGameName = "";
        TotalPlayTime = 0f;
    }

    /// <summary>
    /// Adds points to the player's score.
    /// </summary>
    /// <param name="amount"></param>
    public void AddScore(int amount)
    {
        Score += amount;
    }

    public int GetScore()
    {
        return Score;
    }

    /// <summary>
    /// Removes one life from the player
    /// Returns true if the player has no lives left
    /// </summary>
    /// <returns></returns>
    public bool LoseLife()
    {
        Lives--;

        if (Lives <= 0)
        {
            Lives = 0;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Records that the player completed a minigame
    /// </summary>
    /// <param name="pointsEarned"></param>
    public void MiniGameCompleted(int pointsEarned)
    {
        miniGamesCompleted++;
        AddScore(pointsEarned);
    }

    public void MiniGameFailed()
    {
        miniGamesFailed++;

        bool outOfLives = LoseLife();

        if (outOfLives)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            else if (ProgramSceneManager.Instance != null)
            {
                ProgramSceneManager.Instance.LoadGameOver();
            }
            else
            {
                Debug.LogWarning("No GameManager or ProgramSceneManager found.");
            }
        }
    }

    public void SetCurrentMiniGame(string miniGameName)
    {
        CurrentMiniGameName = miniGameName;
    }
}
