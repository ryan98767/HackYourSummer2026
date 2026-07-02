using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Update()
    {
        if (PlayerDataManager.Instance == null)
        {
            return;
        }

        scoreText.text = $"Score: {PlayerDataManager.Instance.GetScore()}";
    }
}
