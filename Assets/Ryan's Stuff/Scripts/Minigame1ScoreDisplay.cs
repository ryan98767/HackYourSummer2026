using UnityEngine;
using TMPro;

public class Minigame1ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "You saved: " + ScoreManager.Score.ToString() + " fish!";
    }
}
