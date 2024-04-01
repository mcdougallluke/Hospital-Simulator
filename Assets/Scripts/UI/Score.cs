using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public Text MyText;

    // Reference to the ScoreData ScriptableObject
    public ScoreData scoreData;

    void Start()
    {
        // Initialize the score if the ScoreData is null
        if (scoreData == null)
        {
            scoreData = ScriptableObject.CreateInstance<ScoreData>();
            scoreData.score = 0;
        }

        updateScore(0);
    }

    public void Update()
    {
        //testing
        if (Input.GetKey(KeyCode.L))
        {
            // score++;
            updateScore(1);
        }
    }

    /// <summary>
    /// Updates the score count on the GUI, if arg is negative the score will decrease 
    /// by the amount and if positive it will increase by the amount.
    /// </summary>
    public void updateScore(int change)
    {
        scoreData.score += change;
        if (textMeshProUGUI != null && MyText == null)
        {
            textMeshProUGUI.text = "Score: " + scoreData.score;
        }
        else if (textMeshProUGUI == null && MyText != null)
        {
            MyText.text = "Score: " + scoreData.score; // Correctly display the current score
        }
    }

    public int CurrentScore()
    {
        return scoreData.score;
    }
}
