using System;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;

/// <summary>
/// A singular raindrop 
/// </summary>
public class BulletHellRaindrop : MonoBehaviour
{
    /// <summary>
    /// Whether the raindrop will have any physics
    /// </summary>
    [SerializeField] private bool usesPhysics;

    /// <summary>
    /// The direction the raindrop is meant to go
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// The margin in which a raindrop is considered dead off-screen
    /// </summary>
    private const float DespawnMargin = 1f;

    /// <summary>
    /// In p/s, how fast the raindrop will fall
    /// </summary>
    private float speed;

    /// <summary>
    /// Whether or not the raindrop needs to be despawned
    /// </summary>
    private bool needsDespawn;

    
    /// <summary>
    /// Returns whether the raindrop needs to be despawned
    /// </summary>
    public bool NeedsDespawn
    {
        get
        {
            return this.needsDespawn;
        }
    }


    /// <summary>
    /// Allows for the initial values for a raindrop to be created
    /// </summary>
    /// <param name="initPosition">the initial position of the raindrop</param>
    /// <param name="initDirection">the initial direction of the raindrop</param>
    /// <param name="initSpeed">the initial speed of the raindrop</param>
    /// <param name="isStatic">whether the raindrop is static, or will have physics</param>
    public void Initalize(Vector2 initPosition, Vector2 initDirection,
        float initSpeed, bool isStatic = false)
    {
        this.transform.position = initPosition;
        this.direction = initDirection;
        this.speed = initSpeed;
        this.usesPhysics = isStatic;
        this.needsDespawn = false;
    }


    /// <summary>
    /// The actions that should occur each frame, given the right circumstances
    /// </summary>
    /// <param name="dt">A reference to deltatime</param>
    /// <param name="usedCamera">a reference to the camera that's being used</param>
    public void Tick(float dt, Camera usedCamera)
    {
        if (!this.usesPhysics)
        {
            this.transform.position += (Vector3)(direction * speed * dt);

            /*
             * Translates the new position of the raindrop to a viewport point. This also 
             * normalizes the vector3
             */
            Vector3 viewportPoint = usedCamera.WorldToViewportPoint(this.transform.position);

            // Using AABB collision to see whether or not the raindrops are OOB
            if (viewportPoint.x < 0f + DespawnMargin || viewportPoint.x > 1f + DespawnMargin ||
                viewportPoint.y < 0f + DespawnMargin || viewportPoint.y > 1f + DespawnMargin)
            {
                this.needsDespawn = true;
            }
        }
    }
}
