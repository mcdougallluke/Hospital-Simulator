using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI gameOverText; // Used for game over screen
    public Text gamePlayScoreText; // Used for live score during gameplay

    // Reference to the ScoreData ScriptableObject
    public ScoreData scoreData;

    void Start()
    {
        // Initialize the data if ScoreData is null
        if (scoreData == null)
        {
            scoreData = ScriptableObject.CreateInstance<ScoreData>();
            scoreData.score = 0;
            scoreData.patientsSaved = 0;
            scoreData.patientsLost = 0;
        }

        UpdateGameplayScore(); // Initial update to display initial score

        if (gameOverText != null)
        {
            DisplayFinalScores();
        }
    }

    public void UpdateScore(int change)
    {
        scoreData.score += change;
        UpdateGameplayScore();
    }

    public void UpdatePatientsSaved(int count)
    {
        scoreData.patientsSaved += count;
    }

    public void UpdatePatientsLost(int count)
    {
        scoreData.patientsLost += count;
    }

    // Update the gameplay score display
    private void UpdateGameplayScore()
    {
        if (gamePlayScoreText != null)
        {
            gamePlayScoreText.text = $"Score: {scoreData.score}";
        }
    }

    // Call this at game over to display all details
    public void DisplayFinalScores()
    {
        string finalScoreText = $"PATIENTS SAVED: {scoreData.patientsSaved}\n" +
                                $"PATIENTS LOST: {scoreData.patientsLost}\n" +
                                $"FINAL SCORE: {scoreData.score}";

        if (gameOverText != null)
        {
            gameOverText.text = finalScoreText;
        }
    }
}
