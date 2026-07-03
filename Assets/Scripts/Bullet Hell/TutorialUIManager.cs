using TMPro;
using UnityEngine;

/// <summary>
/// Allows the individual control over every manjor UI GameObject in the Tutorial UI CanvasGroup
/// </summary>
public class TutorialUIManager : MonoBehaviour
{
    /// <summary>
    /// A reference to the CanvasGroup that the TutorialUI contains
    /// </summary>
    [Header("Canvas")]
    [SerializeField] private CanvasGroup canvas;

    /// <summary>
    /// Each of the RichTextLabels that are made to follow a particular target
    /// </summary>
    [Header("Follow Texts")]
    [SerializeField] private FollowingLabel playerFollowLabel;
    [SerializeField] private FollowingLabel raindropFollowLabel;

    /// <summary>
    /// Any other fields with special components 
    /// </summary>
    [Header("Other Special Texts")]
    [SerializeField] private TextMeshPro startGameLabel;


    /// <summary>
    /// Presets each of the members of the CanvasGroup
    /// </summary>
    public void Show()
    {
        this.canvas.alpha = 1f;
        this.canvas.interactable = true;
        this.canvas.blocksRaycasts = true;
    }

    /// <summary>
    /// Hides each of the members of the CanvasGroup
    /// </summary>
    public void Hide()
    {
        this.canvas.alpha = 0f;
        this.canvas.interactable = false;
        this.canvas.blocksRaycasts = false;
    }
    
    /// <summary>
    /// Initializes each of the labels, giving them the necessary fields to follow their respective
    /// targets
    /// </summary>
    public void InitializeLabels()
    {
        this.playerFollowLabel.InitFollow();
        this.raindropFollowLabel.InitFollow();
    }

    /// <summary>
    /// Each frame, allows both of the 
    /// </summary>
    public void Tick()
    {
        this.playerFollowLabel.FollowTarget();
        this.raindropFollowLabel.FollowTarget();
    }
}
