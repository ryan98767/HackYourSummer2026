using UnityEngine;

/// <summary>
/// Controls the logic of the TextMeshPro that's following a target in the tutorial
/// </summary>
public class FollowingLabel : MonoBehaviour
{
    /// <summary>
    /// A reference to the targets global position at any point
    /// </summary>
    [SerializeField] Transform target;

    /// <summary>
    /// The camera that's the basis of positioning
    /// </summary>
    [SerializeField] Camera trackedCamera;

    /// <summary>
    /// The default offset the TextMeshPro will have
    /// </summary>
    private Vector3 positionOffset = Vector3.up * 1.5f;

    /// <summary>
    /// A reference to the RectTransform the TextMeshPro has
    /// </summary>
    private RectTransform rect;


    /// <summary>
    /// Initializes the necessary fields prior to updating 
    /// </summary>
    public void InitFollow()
    {
        this.rect = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Updates the position of the TextMeshPro to be that above the player. 
    /// <remarks>This is meant to be called every frame while this is active</remarks>
    /// </summary>
    public void FollowTarget()
    {
        Vector3 desiredWorldPosition = this.target.position + this.positionOffset;
        Vector3 desiredScreenPosition = this.trackedCamera.WorldToScreenPoint(desiredWorldPosition);

        this.rect.position = desiredScreenPosition;
    }
}
