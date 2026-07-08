using UnityEngine;

/// <summary>
/// Detects when the player reaches the exit.
/// Touching the exit wins the minigame
/// </summary>
public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FloodingMiniGameManager.Instance.WinMiniGame();
        }
    }
}
