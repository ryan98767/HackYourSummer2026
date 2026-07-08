using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletHellInGameUIManager : BulletHellCanvasUI
{
    /// <summary>
    /// A reference to the prefab to the heart sprite
    /// </summary>
    [SerializeField] private GameObject heartPrefab;

    /// <summary>
    /// A reference to the current score-keeping label
    /// </summary>
    [SerializeField] private GameObject scoreText;

    /// <summary>
    /// A reference to the container that houses the lives text (and the future hearts)
    /// </summary>
    [SerializeField] private Transform heartsContainer;

    /// <summary>
    /// The component that holds the lerping
    /// </summary>
    private LerpText scoreLerpTextComponent;

    /// <summary>
    /// The component that holds the text in the score GO
    /// </summary>
    private TextMeshProUGUI scoreTextMeshProComponent;

    /// <summary>
    /// A reference to each of the active hearts in the scene
    /// </summary>
    private Queue<GameObject> activeHearts;


    public override void Show()
    {
        base.Show();
        this.gameObject.SetActive(true);
    }

    public override void Tick(float dt)
    {
        this.scoreLerpTextComponent.Tick(dt);
    }

    /// <summary>
    /// Initializes the components necessary for LerpText to work accurately and the additional
    /// UI elements, such as lives and score
    /// </summary>
    /// <param name="initialLives">the lives that the player is starting out with</param>
    /// <see cref="LerpText"/>
    public void Initialize(int initialLives)
    {
        this.scoreLerpTextComponent = this.scoreText.GetComponent<LerpText>();
        this.scoreTextMeshProComponent = this.scoreText.GetComponent<TextMeshProUGUI>();
        this.scoreLerpTextComponent.Initialize();


        SetInitialHearts(initialLives);
        SetInitialScore(0);
    }

    /// <summary>
    /// Sets the initial lives in the start of a play session
    /// </summary>
    /// <param name="numOfHearts">the number of lives the player will have</param>
    /// <remarks>This should only be called in the start of a new game session of the scene. To 
    /// add more hearts, see UpdateHearts</remarks>
    /// <see cref="UpdateHearts"/>sd
    private void SetInitialHearts(int numOfHearts)
    {
        /*
         * It can be assumed, that when this is called, it's only at the start of the game. 
         * As a result, this is a viable option to ensure that activeHearts is always cleared
         */
        this.activeHearts = new Queue<GameObject>();

        while (numOfHearts > 0)
        {
            GameObject newHeart = Instantiate(this.heartPrefab, this.heartsContainer, false);
            this.activeHearts.Enqueue(newHeart);
            numOfHearts--;
        }
    }

    /// <summary>
    /// Sets the score label to the initial score
    /// </summary>
    /// <param name="toWhat">the initial score</param>
    private void SetInitialScore(int toWhat)
    {
        // It can also be assumed here that this is being called at the start of a game
        this.scoreTextMeshProComponent.text = toWhat.ToString();
    }

    /// <summary>
    /// Removes the heart count by one
    /// </summary>
    public void UpdateHearts()
    {
        if (this.activeHearts.TryDequeue(out GameObject removedHeart))
        {
            Destroy(removedHeart);
        }
    }

    /// <summary>
    /// Changes the score's text to a new number
    /// </summary>
    /// <param name="toWhat">the score to be changed into</param>
    public void UpdateScore(float toWhat)
    {
        this.scoreLerpTextComponent.SetTarget(toWhat);
    }
}
