using UnityEngine;

/// <summary>
/// Ends the minigame if the player falls below the camera view
/// </summary>
public class CameraFallDeathCheck : MonoBehaviour
{
    public Transform player;
    public float extraFallDistance = 1f;

    private Camera cam;

    public void Awake()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        if (player == null)
        {
            return;
        }

        float cameraBottomY = transform.position.y - cam.orthographicSize;

        if (player.position.y < cameraBottomY - extraFallDistance)
        {
            FloodingMiniGameManager.Instance.LoseMiniGame();
        }
    }
}
