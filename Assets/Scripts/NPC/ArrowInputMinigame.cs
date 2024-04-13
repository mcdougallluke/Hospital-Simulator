using UnityEngine;

public class ArrowInputMinigame : MonoBehaviour
{
    private int currentIndex = 0;
    private KeyCode[] correctSequence = new KeyCode[6]; // Array to store the sequence
    private bool isMinigameActive = false;

    public void StartMinigame()
    {
        // Randomize the sequence or define a fixed one
        correctSequence = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
        currentIndex = 0;
        isMinigameActive = true;
        Debug.Log("Arrow Input Minigame Started. Follow the sequence!");
    }

    void Update()
    {
        if (isMinigameActive && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(correctSequence[currentIndex]))
            {
                currentIndex++;
                Debug.Log("Correct! Continue...");

                if (currentIndex >= correctSequence.Length)
                {
                    Debug.Log("Sequence complete! Minigame won.");
                    isMinigameActive = false;
                    // You can add score increment or any other success logic here
                }
            }
            else
            {
                Debug.Log("Wrong input! Minigame failed.");
                isMinigameActive = false;
                // Logic to handle failure, e.g., despawning the character
            }
        }
    }
}
