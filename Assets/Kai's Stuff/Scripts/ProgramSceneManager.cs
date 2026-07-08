using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene loading for the game.
/// This script should be used whenever the game needs to move
/// between the main menu, gameplay, minigames, or game over scene
/// </summary>
public class ProgramSceneManager : MonoBehaviour
{
    public static ProgramSceneManager Instance;

    [Header("Scene Names")]
    public string MainMenuScene = "MainMenu";
    public string GameplayScene = "Gameplay";
    public string GameOverScene = "GameOver";

    private void Awake()
    {
        //Singleton setup so other scripts can call this scene manager easily
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //Keeps this object alive when changing scenes
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Loads the main menu
    /// </summary>
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }

    /// <summary>
    /// Loads the main gameplay scene
    /// This can be used in the hub or main controller scene for the minigame
    /// </summary>
    public void LoadGameplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameplayScene);
    }

    /// <summary>
    /// Loads a specific minigame scene by name
    /// Can be called
    /// Pass in your minigame scene to load it
    /// </summary>
    /// <param name="miniGameSceneName"></param>
    public void LoadMiniGame(string miniGameSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(miniGameSceneName);
    }

    /// <summary>
    /// Reloads the current scene
    /// Useful for retrying a minigame or restarting gameplay
    /// </summary>
    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        string currentScene = SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// Loads game over scene
    /// </summary>
    public void LoadGameOver()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameOverScene);

    }

    /// <summary>
    /// Quits the Game
    /// DOESNT WORK IN EDITOR
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
