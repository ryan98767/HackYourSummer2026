using UnityEngine;
using UnityEngine.SceneManagement;

public class Net : MonoBehaviour
{

    public PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            Debug.Log("Caught player");
            playerMovement.Die();

            SceneManager.LoadScene("Minigame1GameOver");
        }
    }
}
