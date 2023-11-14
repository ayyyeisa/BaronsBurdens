/*****************************************************************************
// File Name : AudioManager.cs
// Author : Scott Berry
// Creation Date : November 10, 2023
//
// Brief Description : This manages the audio sources and clips for ease
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //define audio sources from mixer
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    //define sound file sources to be played
    [Header("---------- Audio Clip ----------")]
    public AudioClip Catapult;
    public AudioClip Damage;
    public AudioClip SwordHitPerson;
    public AudioClip SwordMiss;
    public AudioClip WingBeat;
    public AudioClip Fireball;
    public AudioClip SwordHitShield;
    public AudioClip SwordHitSword;
    public AudioClip Click;

    /// <summary>
    /// Play backround music upon scene start
    /// </summary>
    private void Start()
    {
        musicSource.Play();
    }

    /// <summary>
    /// Play given SFX when speciefied
    /// </summary>
    /// <param name="clip">Desired SFX</param>
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
