using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI gameOverText; // Used for game over screen
    public Text gamePlayScoreText; // Used for live score during gameplay

    public TextMeshProUGUI highScoreText; // Used for high score display

    public GameObject scoreChangePrefab;
    public Transform scoreParent;
    public RectTransform endPosition;
    public ScoreData scoreData;

    private bool newHighScore = false;

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
        newHighScore = false;
    }

    public void UpdateScore(int change)
    {
        scoreData.score += change;
        var inst = Instantiate(scoreChangePrefab, Vector3.zero, Quaternion.identity);
        inst.transform.SetParent(scoreParent, false);
        RectTransform rect = inst.GetComponent<RectTransform>();

        Text text = inst.GetComponent<Text>();
        text.text = (change > 0 ? "+" : "") + change.ToString();
        text.color = change > 0 ? Color.green : Color.red;

        LeanTween.moveY(rect, endPosition.anchoredPosition.y, 1f).setOnComplete(() => {
            Destroy(inst);
            UpdateGameplayScore();
        });
        LeanTween.alphaText(rect, 0.5f, 1f);
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
        CheckHighScore();

        string finalScoreText = $"PATIENTS SAVED: {scoreData.patientsSaved}\n" +
                                $"PATIENTS LOST: {scoreData.patientsLost}\n" +
                                $"FINAL SCORE: {scoreData.score}";

        if (gameOverText != null)
        {
            gameOverText.text = finalScoreText;
        }

        if (newHighScore)
        {
            highScoreText.enabled = true;
        } else
        {
            highScoreText.enabled = false;
        }
    }

    private void CheckHighScore()
    {
        if (scoreData.score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", scoreData.score);
            Debug.Log("New High Score: " + scoreData.score);
            newHighScore = true;
        }
    }
}
