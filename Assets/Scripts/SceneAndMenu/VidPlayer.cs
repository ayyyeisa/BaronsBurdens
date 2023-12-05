/*****************************************************************************
// File Name : VidPlayer.cs
// Author : Isa Luluquisin
// Creation Date : November 28, 2023
//
// Brief Description : This is a file that handles the loading of main menu
                        after the intro scene-- specifically used for the webbuild.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{
    [Tooltip("Filename of the video that must be played")]
    [SerializeField] string videoFileName;

    void Start()
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        //plays the video if there is a videoplayer present
        if(videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
            videoPlayer.loopPointReached += LoadScene;
        }

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
