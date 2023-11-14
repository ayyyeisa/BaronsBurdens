/*****************************************************************************
// File Name : IntroController.cs
// Author : Isa Luluquisin
// Creation Date : October 23, 2023
//
// Brief Description : This is a file that handles the loading of main menu
                        after the intro scene
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroController : MonoBehaviour
{
    [Tooltip("Video that will be played at the start of the scene")]
    [SerializeField] private VideoPlayer video;

    void Start()
    {
        //calls on LoadScene() once video finishes
        video.loopPointReached += LoadScene;
    }
    /// <summary>
    /// Loads the main menu scene once the video has ended
    /// </summary>
    /// <param name="video">introduction video that is played on screen </param>
    void LoadScene(VideoPlayer video)
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
