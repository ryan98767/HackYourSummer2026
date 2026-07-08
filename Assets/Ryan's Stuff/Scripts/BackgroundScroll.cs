using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private float resetPoint = -20f;  // how far left before resetting
    [SerializeField] private float startPoint = 20f;   // where it resets to

    [SerializeField] private Minigame1CameraMovement cameraMovement;

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.left * cameraMovement.ScrollSpeed * Time.deltaTime);

        if (transform.position.x < resetPoint)
        {
            transform.position = new Vector3(startPoint, transform.position.y, transform.position.z);
        }
    }
}
