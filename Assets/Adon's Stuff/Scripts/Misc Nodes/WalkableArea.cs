using System;
using UnityEngine;

/// <summary>
/// An area that can be walked into by something to trigger something
/// </summary>
public class WalkableArea : MonoBehaviour
{
    /// <summary>
    /// Invoked when the target has entered the target area
    /// </summary>
    public event Action OnTargetEntered;


    /// <summary>
    /// The desired tag that's being searched for
    /// </summary>
    [SerializeField] private string targetTag;

    /// <summary>
    /// Checks whether whatever collided with the walkable area is what it's looking for
    /// </summary>
    /// <param name="collision">the collider that collided with the WalkableArea</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            this.OnTargetEntered.Invoke();
        }
    }
}
