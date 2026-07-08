using TMPro;
using UnityEngine;

/// <summary>
/// Controls the main systems behind receiving a compliment following the BH gell
/// </summary>
public class ComplimentText : MonoBehaviour
{
    /// <summary>
    /// The "Compliments" that the player gets for getting a certain score
    /// </summary>
    /// <remarks>are entirely dependent on how high of a score they got. I decided, that 
    /// I'd rank the intensity of the insulting/glazing to how bad/well the 
    /// player performed. 1-4 are straight up insults, 5-7 are mid remarks, while 
    /// 8-10 are just straight glazing remarks</remarks>
    [Header("Straight up player insults (1-4)")]
    [SerializeField] private string statementOne;
    [SerializeField] private string statementTwo;
    [SerializeField] private string statementThree;
    [SerializeField] private string statementFour;

    [Header("Mid Results")]
    [SerializeField] private string statementFive;
    [SerializeField] private string statementSix;
    [SerializeField] private string statementSeven;

    [Header("Glazing Compliments")]
    [SerializeField] private string statementEight;
    [SerializeField] private string statementNine;
    [SerializeField] private string statementTen;

    [Header("Statement Mesh")]
    [SerializeField] private TextMeshProUGUI label;


    /// <summary>
    /// Updates the compliment text every frame
    /// </summary>
    /// <param name="currentScore">a reference to the current score that's given</param>
    public void Tick(int currentScore)
    {
        SetComplimentText(currentScore);
    }

    /// <summary>
    /// Sets the compliment text depending on the score given
    /// </summary>
    /// <param name="currentScore"></param>
    /// <remarks>Given the things I've also made, this is def one of the lazier approaches, but
    /// I'm pretty tired.</remarks>
    private void SetComplimentText(int currentScore)
    {
        switch (currentScore)
        {
            case (< 100):
                this.label.text = statementOne;
                break;
            case (< 200):
                this.label.text = statementTwo;
                break;
            case (< 300):
                this.label.text = statementThree;
                break;
            case (< 400):
                this.label.text = statementFour;
                break;
            case (< 600):
                this.label.text = statementFive;
                break;
            case (< 800):
                this.label.text = statementSix;
                break;
            case (< 1000):
                this.label.text = statementSeven;
                break;
            case (< 1200):
                this.label.text = statementEight;
                break;
            case (< 1300):
                this.label.text = statementNine;
                break;
            case (> 1400):
                this.label.text = statementTen;
                break;
        }
    }
}
