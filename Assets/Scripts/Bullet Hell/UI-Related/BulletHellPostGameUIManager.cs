using TMPro;
using UnityEngine;

/// <summary>
/// The manager to process and control UI
/// </summary>
public class BulletHellPostGameUIManager : BulletHellCanvasUI
{
    /// <summary>
    /// A reference to the lerping text component that's being used under the CanvasGroup
    /// </summary>
    [SerializeField] private LerpText lerpTextComponent;

    /// <summary>
    /// A reference to the TextMeshPro component that's being used under the CanvasGroup
    /// </summary>
    [SerializeField] private TextMeshProUGUI lerpTextMeshProComponent;

    /// <summary>
    /// A reference to the compliment text componenet being used
    /// </summary>
    [SerializeField] private ComplimentText complimentText;

    /// <summary>
    /// How long it'll take before the buildup is finished to begin the lerping in seconds
    /// </summary>
    [SerializeField] private float timeBeforeBuildupFinish;

    /// <summary>
    /// Whether the text is ready to begin going up.
    /// </summary>
    /// <remarks>this is done to give the player some sort of build up before they get their final
    /// score</remarks>
    private bool readyToBuildUp;

    /// <summary>
    /// A reference to the final score that the player got
    /// </summary>
    private float finalScore;

    /// <summary>
    /// How long the buildup to the beginning of the larping has been delayed
    /// </summary>
    private float buildupAccumulator;


    /// <summary>
    /// Truly begins the buildup before the lerping of the label's values begin
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    private void StartBuildup(float dt)
    {
        this.buildupAccumulator += dt;

        if (this.buildupAccumulator >= this.timeBeforeBuildupFinish)
        {
            this.lerpTextComponent.Initialize();
            this.lerpTextComponent.SetTarget(this.finalScore);
            this.readyToBuildUp = true;
        }
    }

    /// <summary>
    /// Grabs references to the text components while also setting each of the necessary default values
    /// </summary>
    /// <param name="finalScore">the score that the lerping will eventually get to</param>
    public void Initialize(float finalScore)
    {
        this.finalScore = finalScore;
        this.lerpTextMeshProComponent.text = 0.ToString();
    }

    /// <summary>
    /// Allows the text to begin lerping once it's time to
    /// </summary>
    /// <param name="dt">a reference to deltatime</param>
    public override void Tick(float dt)
    {
        if (this.readyToBuildUp)
        {
            // The lerping should go first to give the player an immediate update
            this.lerpTextComponent.Tick(dt);

            // Now the score should be updated and considered for the compliment text
            int currentScore = int.Parse(this.lerpTextMeshProComponent.text);
            this.complimentText.Tick(currentScore);
        }
        else
        {
            StartBuildup(dt);
        }
    }

    public override void Show()
    {
        base.Show();
        this.gameObject.SetActive(true);
    }
}
