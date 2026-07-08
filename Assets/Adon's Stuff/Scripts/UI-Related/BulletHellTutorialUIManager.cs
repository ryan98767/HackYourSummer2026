using TMPro;
using UnityEngine;

/// <summary>
/// Allows the individual control over every manjor UI GameObject in the Tutorial UI CanvasGroup
/// </summary>
public class BulletHellTutorialUIManager : BulletHellCanvasUI
{
    /// <summary>
    /// Each of the RichTextLabels that are made to follow a particular target
    /// </summary>
    [Header("Follow Texts")]
    [SerializeField] private FollowingLabel playerFollowLabel;
    [SerializeField] private FollowingLabel raindropFollowLabel;

    [Header("Other Nodes")]
    [SerializeField] private BulletHellRaindrop showcaseRaindrop;
    
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
    /// Presets each of the members of the CanvasGroup while also showing
    /// the showcase raindrop
    /// </summary>
    public override void Show()
    {
        base.Show();
        this.showcaseRaindrop.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides each of the members of the CanvasGroup while also destroying the
    /// showcase raindrop
    /// </summary>
    public override void Hide()
    {
        base.Hide();
        this.showcaseRaindrop.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the positions of each of the necessary follow labels
    /// </summary>
    public override void Tick(float dt)
    {
        this.playerFollowLabel.FollowTarget();
        this.raindropFollowLabel.FollowTarget();
    }
}
