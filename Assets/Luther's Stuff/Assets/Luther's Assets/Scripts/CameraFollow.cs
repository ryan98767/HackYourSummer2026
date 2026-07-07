using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      
    public float smoothTime = 0.2f;
    public Vector3 offset = new Vector3(0, 1f, -10f);
    public float minX = -100f;    
    public float maxX = 100f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
