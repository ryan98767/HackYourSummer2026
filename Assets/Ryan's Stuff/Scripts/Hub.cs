using UnityEngine;
using UnityEngine.SceneManagement;

public class Hub : MonoBehaviour
{
    private HubObject currentMinigame = null;
    [SerializeField] GameObject speechBubble;

    void Start()
    {
        speechBubble.SetActive(false);
    }

    void Update()
    {
        if (currentMinigame != null && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(currentMinigame.sceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HubObject minigame = collision.GetComponent<HubObject>();
        if (minigame != null)
        {
            currentMinigame = minigame;
            speechBubble.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HubObject minigame = collision.GetComponent<HubObject>();
        if (minigame != null)
        {
            speechBubble.SetActive(false);
            currentMinigame = null;
        }
    }
}
