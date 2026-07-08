using UnityEngine;

public class Animal : MonoBehaviour
{
    public GameObject rescueEffectPrefab; 

    private bool rescued = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (rescued) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            rescued = true;
            if (rescueEffectPrefab != null)
            {
                Instantiate(rescueEffectPrefab, transform.position, Quaternion.identity);
            }
            player.PickUpAnimal(gameObject);
        }
    }
}
