using UnityEngine;

/// <summary>
/// Base class to represent a "Screen" of UI during the minigame
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public abstract class BulletHellCanvasUI : MonoBehaviour
{
    /// <summary>
    /// A reference to the CanvasGroup that the UI group contains
    /// </summary>
    [Header("Canvas")]
    [SerializeField] protected CanvasGroup canvasGroup;


    /// <summary>
    /// Presets each of the members of the CanvasGroup
    /// </summary>
    public virtual void Show()
    {
        this.canvasGroup.alpha = 1f;
        this.canvasGroup.interactable = true;
        this.canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Hides each of the members of the CanvasGroup
    /// </summary>
    public virtual void Hide()
    {
        this.canvasGroup.alpha = 0f;
        this.canvasGroup.interactable = false;
        this.canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Allows for anything to occur UI-wise that may need to happen
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    public virtual void Tick(float dt)
    {
    }
}
