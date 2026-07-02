using UnityEngine;

/// <summary>
/// Moves the water upward over time
/// If the water reaches the player, the player loses
/// </summary>
public class RisingWater : MonoBehaviour
{
    [Header("Water Settings")]
    public float riseSpeed = 1f;
    
    private void Update()
    {
        if (FloodingMiniGameManager.Instance == null)
        {
            return;
        }

        if (!FloodingMiniGameManager.Instance.gameIsActive)
        {
            return;
        }

        //Move the water upward every frame
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If the water touches the player, the minigame is lost
        if (other.CompareTag("Player"))
        {
            FloodingMiniGameManager.Instance.LoseMiniGame();
        }
    }
}
