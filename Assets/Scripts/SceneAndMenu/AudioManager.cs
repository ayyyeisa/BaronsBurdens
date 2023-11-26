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
    [Tooltip("Music object containing backround music")]
    [SerializeField] private AudioSource musicSource;
    [Tooltip("SFX object")]
    [SerializeField] private AudioSource SFXSource;

    //define sound file sources to be played
    [Header("---------- SFX Clips ----------")]
    [Tooltip("SFX for Catapult Launch")]
    public AudioClip Catapult;
    [Tooltip("SFX for Enemy Knight defeated")]
    public AudioClip Damage;
    [Tooltip("SFX for when player hits enemy in duel")]
    public AudioClip SwordHitPerson;
    [Tooltip("SFX for when player misses in duel")]
    public AudioClip SwordMiss;
    [Tooltip("SFX for when the player dragon moves")]
    public AudioClip WingBeat;
    [Tooltip("SFX for when player shoots fireball")]
    public AudioClip Fireball;
    [Tooltip("SFX for when player blocks")]
    public AudioClip SwordHitShield;
    [Tooltip("SFX for when player parries")]
    public AudioClip SwordHitSword;
    [Tooltip("SFX for when player clicks buttons")]
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
