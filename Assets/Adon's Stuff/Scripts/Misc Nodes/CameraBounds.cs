using UnityEngine;

/// <summary>
/// The boundaries for the camera at any given frame
/// </summary>
public struct CameraBounds
{
    /// <summary>
    /// The position of the center-left bounds of the camera
    /// </summary>
    public float left;

    /// <summary>
    /// The position of the center-right bounds of the camera
    /// </summary>
    public float right;

    /// <summary>
    /// The position of the center-top bounds of the camera
    /// </summary>
    public float top;

    /// <summary>
    /// The position of the center-bottom bounds of the camera
    /// </summary>
    public float bottom;
}
