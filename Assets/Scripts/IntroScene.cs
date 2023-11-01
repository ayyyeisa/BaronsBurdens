/// <summary>
/// 
/// Author: Isa Luluquisin
/// Date: October 23, 2023
/// 
/// Description: This is a file that works on most controls for the 
/// Dragon Riding minigame, as well as spawning in player and enemy fireballs
/// and implementing the Game timer
/// 
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroScene : MonoBehaviour
{
    // Behavior for Intro Scene to load main menu scene once intro scene is done
    [SerializeField] private VideoPlayer video;

    void Start()
    {
        video.loopPointReached += LoadScene;
    }
    void LoadScene(VideoPlayer video)
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
