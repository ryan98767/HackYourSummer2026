using UnityEngine;

/// <summary>
/// Gives score once when the player lands on this platform.
/// Attach this to the PlatformPrefab.
/// </summary>
public class PlatformScore : MonoBehaviour
{
    public int points = 1;
    private bool scoreGiven = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (scoreGiven)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            scoreGiven = true;

            if (PlayerDataManager.Instance != null)
            {
                PlayerDataManager.Instance.AddScore(points);
            }
        }
    }



}