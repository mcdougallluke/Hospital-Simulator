using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
{
    public float timeValue = 20; //300 (5 minutes)
    public Text timerText;
    public Image endGameFadePanel; // Reference to the fade panel
    public Text scoreText; // Reference to display score at the end
    public Score scoreScript; // Reference to the Score script

    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;

            StartCoroutine(EndGameSequence());
           
        }
        DisplayTime(timeValue);
    }

    IEnumerator EndGameSequence()
    {
        // Disable further updates to prevent this coroutine from being called again.
        this.enabled = false;
        
        // Optionally hide or disable game objects here
        
        // Fade to black
        float fadeDuration = 2.0f; // Duration of the fade
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime / fadeDuration;
            endGameFadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        SceneManager.LoadScene(3);
        // Display the final score
        // scoreText.text = "Final Score: " + scoreScript.CurrentScore();
        // scoreText.gameObject.SetActive(true);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
