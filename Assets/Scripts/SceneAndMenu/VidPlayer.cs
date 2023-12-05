using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{
    [SerializeField] string videoFileName;

    void Start()
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

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
