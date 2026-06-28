using UnityEngine;
using UnityEngine.SceneManagement;

public class SetMovement : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private string currentMinigame; 

    private bool sideScroll;
    void Start()
    {
        //a manager to set the control scheme for each minigame, add this to an empty game manager in each minigame
        currentMinigame = SceneManager.GetActiveScene().name;

        // Set the control scheme based on the current minigame, add your minigames here and set the control scheme for each one once implemented
        switch (currentMinigame) {
            case "Minigame 1":
                sideScroll = false;
                Debug.Log("Setting scheme for Minigame 1");
                //SideScroll should be capitalized, including the first s
                playerMovement.SideScroll = sideScroll;
                break;

            default: //default would be the same as hub, for now it's side scroll but depending on how the hub works out it could switch
                Debug.Log("Setting scheme for default");
                playerMovement.SideScroll = true;
                break;
        }
    }
}
