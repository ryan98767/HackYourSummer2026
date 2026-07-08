using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //level time 
    public float levelTimeSeconds = 90f;

    //score tracking stuff
    public bool trackBestScore = true;
    private const string BestScoreKey = "WildfireRescue_BestScore";

    //ui stuff
    public TMP_Text firesText;
    public TMP_Text animalsText;
    public TMP_Text waterText;
    public TMP_Text timerText;
    public GameObject resultsPanel;//shown when the timer hits zero
    public TMP_Text resultsSummaryText; 
    public TMP_Text bestScoreText;

    //ref to player
    public PlayerController player;

    private int firesExtinguished = 0;
    private int animalsRescued = 0;
    private float timeRemaining;
    private bool gameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeRemaining = levelTimeSeconds;
        if (resultsPanel != null) resultsPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (gameOver) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndGame();
        }

        if (waterText != null && player != null)
        {
            waterText.text = "Water: " + player.currentWater + " / " + player.maxWater;
        }
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
        }
    }

    public void OnFireExtinguished()
    {
        firesExtinguished++;
        UpdateUI();
    }

    public void OnAnimalsDroppedOff(int count)
    {
        animalsRescued += count;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (firesText != null) firesText.text = "Fires Out: " + firesExtinguished;
        if (animalsText != null) animalsText.text = "Animals Saved: " + animalsRescued;
    }

    void EndGame()
    {
        gameOver = true;

        int score = firesExtinguished + animalsRescued;
        int best = score;

        if (trackBestScore)
        {
            int previousBest = PlayerPrefs.GetInt(BestScoreKey, 0);
            best = Mathf.Max(previousBest, score);
            PlayerPrefs.SetInt(BestScoreKey, best);
            PlayerPrefs.Save();
        }

        if (resultsSummaryText != null)
        {
            resultsSummaryText.text = $"Fires put out: {firesExtinguished}\nAnimals saved: {animalsRescued}";
        }
        if (bestScoreText != null && trackBestScore)
        {
            bestScoreText.text = "Best score: " + best;
        }

        if (resultsPanel != null) resultsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}