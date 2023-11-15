/*****************************************************************************
// File Name : PauseMenuScript.cs
// Author : Scott Berry
// Creation Date : November 7, 2023
//
// Brief Description : This has all of the button functionality for the Pause menu
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenuScript : MonoBehaviour
{
    
    [Tooltip("All Panels in Pause Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [Tooltip("The instruction at the beginning of the game")]
    [SerializeField] private GameObject howToPlayMenu;
    
    [Tooltip("Bool to check if game is paused")]
    private bool isPaused;



    // Start is called before the first frame update
    void Start()
    {
        //initialize the panels to be inactive
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Pause the game when P key is pressed and set isPaused to true
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            PauseGame();
            isPaused = true;
        }
        //Unpause the game when P key is pressed again and set isPaused to false
        else if (Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            ResumeGame();
            isPaused = false;
        }

    }

    /// <summary>
    /// Active pause menu and freeze time
    /// </summary>
    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    /// <summary>
    /// Deactive pause menu and resume time
    /// </summary>
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// AChnage to main menu scene and resume time
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Close out of the game completely
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Reset the level and resume time
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Activate settings menu
    /// </summary>
    public void Settings()
    {
        settingsMenu.SetActive(true);
    }

    /// <summary>
    /// This brings the screen back to the Pause Menu 
    /// </summary>
    public void BackButton()
    {
        settingsMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
    }

    /// <summary>
    /// Activate How To Play Menu
    /// </summary>
    public void HowToPlayButton()
    {
        howToPlayMenu.SetActive(true);
    }


    /// <summary>
    /// This Opens the mechanics Menu
    /// </summary>
    public void OpenMechanics()
    {
        pauseMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
    }


    /// <summary>
    /// This opens the Enemies Menu
    /// </summary>
    public void Enemies()
    {
        pauseMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
    }

    /// <summary>
    /// Sets pause menu and howtoplay to false
    /// </summary>
    public void Features()
    {
        pauseMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
    }


    /// <summary>
    /// This is what happens when you click the back button on each of the panels
    /// </summary>
    public void BackButtonHowToPlay()
    {
        pauseMenu.SetActive(true);
        howToPlayMenu.SetActive(false);
    }
}
