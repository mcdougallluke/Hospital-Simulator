using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text MyText;
    private int score = 10;

    // Start is called before the first frame update
    void Start()
    {
        updateScore(0);
    }

    /// <summary>
    /// Updates the score count on the GUI, if arg is negative the score will decrease 
    /// by the amount and if positive it will increase by the amount.
    /// </summary>
    void updateScore(int change)
    {
        score += change;
        MyText.text += " " + score;

    }
}
