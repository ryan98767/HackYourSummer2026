using TMPro;
using UnityEngine;

/// <summary>
/// A component to certain Labels that allows the text to lerp
/// </summary>
public class LerpText : MonoBehaviour
{
    /// <summary>
    /// The speed of which the lerping will occur. The higher, the faster
    /// </summary>
    [SerializeField] private float lerpSpeed;

    /// <summary>
    /// The text component that's apart of the RichTextLabel
    /// </summary>
    private TextMeshProUGUI associatedText;

    /// <summary>
    /// The current score of the game
    /// </summary>
    private float currentScore;

    /// <summary>
    /// The expected score the lerping needs to get to
    /// </summary>
    private float targetScore;


    /// <summary>
    /// The actions that'll occur once the label is needed in the scene
    /// </summary>
    public void Initialize()
    {
        this.associatedText = this.gameObject.GetComponent<TextMeshProUGUI>();

        /*
         * Parsed instead of hardcoded. Maybe--just MAYBE, I fixed it so 
         * I didn't have to hard code it in the InGameState, so just to be safe.
         */
        float.TryParse(this.associatedText.text, out this.currentScore);

        /*
         * Made way too many mistakes forgetting that the target score needs to be the init current
         * score
         */
        this.targetScore = this.currentScore;
    }

    /// <summary>
    /// What happens every frame during the game
    /// </summary>
    /// <param name="dt">A reference to deltatime</param>
    public void Tick(float dt)
    {
        this.currentScore = Mathf.Lerp(this.currentScore, this.targetScore, this.lerpSpeed * dt);
        this.associatedText.text = Mathf.RoundToInt(currentScore).ToString();
    }

    /// <summary>
    /// Sets a the target score to a particular value, allowing it to start being lerped
    /// </summary>
    /// <param name="toWhat">the value the target to get set to</param>
    public void SetTarget(float toWhat)
    {
        this.targetScore = Mathf.FloorToInt(toWhat);
    }
}
