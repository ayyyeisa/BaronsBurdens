/*****************************************************************************
// File Name : Menu.cs
// Author : Carl Crumer
// Creation Date : October 23, 2023
//
// Brief Description : This is a file that connects all the scenes together, 
                       as well as allow for the skipping of the introduction 
                       scene.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    /// <summary>
    /// Loads the dragon scene when the dragon button is clicked
    /// </summary>
    public void PlayDragon()
    {
        SceneManager.LoadScene(2);
    }
    /// <summary>
    /// Loads the catapult scene when the catapult button is clicked
    /// </summary>
    public void PlayCatapult()
    {
        SceneManager.LoadScene(3);
    }
    /// <summary>
    /// Loads the duel scene when the catapult button is clicked
    /// </summary>
    public void PlayDuel()
    {
        SceneManager.LoadScene(4);

    }
    /// <summary>
    /// Quits the game on selection of esc
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    /// <summary>
    /// Skips the intro scene when the skip button is pressed in intro scene
    /// </summary>
    public void SkipIntroScene()
    {
        SceneManager.LoadScene(1);
    }


}
