using UnityEngine;


public class ScoreManager : MonoBehaviour
{

    private static int score;

    public static int Score
    {
        get { return score; }
        set { score = value; }
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }

    public static void ResetScore()
    {
        score = 0;
    }
}
