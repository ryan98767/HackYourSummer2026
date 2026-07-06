using UnityEngine;
using UnityEngine.SceneManagement;

public class Net : MonoBehaviour
{

    public PlayerMovement playerMovement;
    public Minigame1CameraMovement cameraMovement;

    [SerializeField] private int netHealth = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            Debug.Log("Caught player");
            playerMovement.Die();

            SceneManager.LoadScene("Minigame1GameOver");
        }
        
        //each time they collide with a specific rock, the net will take damage and slow.
        //If the player can last long enough, its a way to win outside of simply runnning until dying
        if (collision.CompareTag("BigRock"))
        {
            Debug.Log("Net took damage");
            cameraMovement.ScrollSpeed -= 2f;
            netHealth--;

            if (netHealth <= 0)
            {
                SceneManager.LoadScene("Minigame1GameOver");
            }
        }
    }
}
