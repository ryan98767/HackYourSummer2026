using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the remaining time for the flooding minigame.
/// </summary>
public class TimerUI : MonoBehaviour
{
    public Text timerText;
    
    void Update()
    {
        if (FloodingMiniGameManager.Instance == null)
        {
            return;
        }

        float timeLeft = FloodingMiniGameManager.Instance.GetCurrentTime();

        if (timeLeft < 0)
        {
            timeLeft = 0;
        }

        timerText.text = $"Time: {timeLeft.ToString("F1")}";
    }
}
