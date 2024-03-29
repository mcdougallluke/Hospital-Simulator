using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /*AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }*/

    public void PlayGame()
    {
        Cursor.visible = true;
        SceneManager.LoadScene(1);
    }
    public void Credits()
    {
        Debug.Log("Going to credits scene.");
        SceneManager.LoadScene(2);
    }
    public void QuitGame()
    { 
        Application.Quit();
        Debug.Log("Quiting Game.");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}