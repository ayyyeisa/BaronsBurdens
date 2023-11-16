/*****************************************************************************
// File Name : VolumeSettings.cs
// Author : scott Berry
// Creation Date : November 10, 2023
//
// Brief Description : This has all the functionality for the sliders that control Music and SFX volume
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    
    [Tooltip("Game Mixer")]
    [SerializeField] private AudioMixer myMixer;

    
    [Tooltip("Sliders for Sound FX and Music")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    
    [Tooltip("The Settings Menu")]
    [SerializeField] private GameObject SettingsMenu;


    private void Start()
    {
        //set volume to initial level
        SetMusicVolume();
    }

    /// <summary>
    /// uses slider volume to change volume in mixer for just music
    /// </summary>
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    /// <summary>
    /// uses slider volume to change volume in mixer for just SFX
    /// </summary>
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

}

