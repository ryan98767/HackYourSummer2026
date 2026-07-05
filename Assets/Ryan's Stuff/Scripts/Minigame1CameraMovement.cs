using UnityEngine;

public class Minigame1CameraMovement : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float speedIncrement = 0.5f;
    [SerializeField] private float maxSpeed = 20f;

    // Update is called once per frame
    void Update()
    {
        scrollSpeed = Mathf.Min(scrollSpeed + speedIncrement * Time.deltaTime, maxSpeed);

        //Debug.Log("Current Speed: " + scrollSpeed.ToString());
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }
}
