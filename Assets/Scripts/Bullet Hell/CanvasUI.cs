using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
/// <summary>
/// Represents all canvases that are in any scene for encapsulation
/// </summary>
public class CanvasUI : MonoBehaviour
{
    /// <summary>
    /// A reference to the group of UI the canvas has
    /// </summary>
    private CanvasGroup canvasGroup;


    /// <summary>
    /// Returns a reference to the associated CanvasGroup
    /// </summary>
    public CanvasGroup CanvasGroup
    {
        get
        {
            return this.canvasGroup;
        }
    }


    /// <summary>
    /// Initializes the CanvasGroup and ensures that it exists
    /// </summary>
    public void Awake()
    {
        this.canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Makes the associated CanvasGroup able to be interacted with and visible
    /// </summary>
    public void Show()
    {
        this.canvasGroup.alpha = 1f;
        this.canvasGroup.interactable = true;
        this.canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Makes the associated CanvasGroup unable to be interacted with and invisible
    /// </summary>
    public void Hide()
    {
        this.canvasGroup.alpha = 0f;
        this.canvasGroup.interactable = false;
        this.canvasGroup.blocksRaycasts = false;
    }
}
