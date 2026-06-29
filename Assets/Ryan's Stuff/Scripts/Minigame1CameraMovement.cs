using UnityEngine;

public class Minigame1CameraMovement : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }
}
