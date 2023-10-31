using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroScene : MonoBehaviour
{
    // Behavior for Intro Scene to load main menu scene once intro scene is done
    VideoPlayer video;

    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        StartCoroutine("IntroEnd");
    }

    public IEnumerator IntroEnd()
    {
        while (video.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    void OnVideoEnd()
    {
        SceneManager.LoadScene(1);
    }
}
