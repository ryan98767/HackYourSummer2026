using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Represents every major state the game can be in
/// Additional states can be added here later if needed
/// </summary>
namespace Minigame2GameState
{
    public enum GameState
    {
        MainMenu,
        Gameplay,
        Paused,
        GameOver
    }


    /// <summary>
    /// Controls the overall game state (Main Menu, Gameplay, Pause, and Game Over).
    /// This should be the only script responsible for switching between game states
    /// </summary>

    public class GameManager : MonoBehaviour
    {
        //Singleton instance so other scripts can access the GameManager
        public static GameManager Instance;

        //Stores the current state of the game
        public GameState CurrentState { get; private set; }

        [Header("UI Screens")]

        //References to each UI canvas/panel
        //Only one of these should be active at a time.
        public GameObject MainMenuUI;
        public GameObject GameplayUI;
        public GameObject PauseUI;
        public GameObject GameOverUI;

        private void Awake()
        {
            //Make sure there is only one GameManager in the game
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            //Keeps this GameManager alive when loading new scenes.
            DontDestroyOnLoad(gameObject);
        }


        private void Start()
        {
            //The game always starts on the Main Menu
            ChangeState(GameState.MainMenu);
        }


        private void Update()
        {
            //Allow the Escape key to toggle pause while playing
            if (CurrentState == GameState.Gameplay && Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeState(GameState.Paused);
            }

            //Pressing Esc again resumes gameplay
            else if (CurrentState == GameState.Paused && Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeState(GameState.Gameplay);
            }
        }

        /// <summary>
        /// Changes the current game state and updates
        /// the UI and game time accordingly
        /// </summary>
        /// <param name="newState">The state to switch to</param>
        public void ChangeState(GameState newState)
        {
            //store the new state
            CurrentState = newState;

            //Turn off every UI screen before enabling the correct one
            //This prevents multiple menus from showing at once
            MainMenuUI.SetActive(false);
            GameplayUI.SetActive(false);
            PauseUI.SetActive(false);
            GameOverUI.SetActive(false);

            //Resume time by default
            //States that need the game frozen will override this
            Time.timeScale = 1f;

            switch (newState)
            {
                case GameState.MainMenu:
                    {
                        //Displays the main menu
                        MainMenuUI.SetActive(true);
                        break;
                    }
                case GameState.Gameplay:
                    {
                        //Displays gameplay HUD
                        GameplayUI.SetActive(true);
                        break;
                    }
                case GameState.Paused:
                    {
                        //Show pause menu and freeze gameplay
                        PauseUI.SetActive(true);
                        Time.timeScale = 0f;
                        break;
                    }
                case GameState.GameOver:
                    {
                        //Show game over screen and stop gameplay
                        GameOverUI.SetActive(true);
                        Time.timeScale = 0f;
                        break;
                    }
            }
        }

        /// <summary>
        /// Called when the player presses the Start button
        /// Switches from the main menu to gameplay
        /// </summary>
        public void StartGame()
        {
            ChangeState(GameState.Gameplay);
        }

        /// <summary>
        /// Called by the Resume button on the pause menu
        /// Returns the player to gameplay
        /// </summary>
        public void ResumeGame()
        {
            ChangeState(GameState.Gameplay);
        }

        /// <summary>
        /// Can be called by any minigame or health system
        /// When the player loses
        /// </summary>
        public void GameOver()
        {
            ChangeState(GameState.GameOver);
        }

        /// <summary>
        /// Reloads the current scene so the game can be played again
        /// </summary>
        public void RestartGame()
        {
            //Make sure time is running before loading
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Exits the app
        /// Note: This only works in a built verson of the game 
        /// DOES NOT WORK WHILE RUNNING IN THE EDITOR
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
