using UnityEngine;
using WildfireGame;


public class SafeZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && player.CarriedCount > 0)
        {
            int rescuedCount = player.DropOffAnimals();
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnAnimalsDroppedOff(rescuedCount);
            }
        }
    }
}
