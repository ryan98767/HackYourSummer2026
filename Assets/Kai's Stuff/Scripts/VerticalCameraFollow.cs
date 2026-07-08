using UnityEngine;

/// <summary>
/// Makes the camera scroll upward with the player
/// The camera only moves up, so the player cannot move the camera downward
/// </summary>
public class VerticalCameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Camera Settings")]
    public float followSpeed = 3f;
    public float yOffset = 2f;

    private float highestCameraY;

    private void Start()
    {
        highestCameraY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        if (FloodingMiniGameManager.Instance != null && !FloodingMiniGameManager.Instance.gameIsActive)
        {
            return;
        }

        float targetY = player.position.y + yOffset;

        //Only allow the camera to move upward
        if (targetY > highestCameraY)
        {
            highestCameraY = targetY;
        }

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            highestCameraY,
            transform.position.z
            );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
            );
    }

    
}
