/*****************************************************************************
// File Name : EnemyKnightt.cs
// Author : Tri Nguyen
// Creation Date : November 27, 2023
//
// Brief Description : This is a file that works on the behavior for the slower
// enemy knight prefabs
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowerEnemyKnightBehaviour : MonoBehaviour
{
    //private HealthBarBehaviour healthSlider;
    private int enemyLives = 2;
    [SerializeField] private Slider healthBarSlider;
    //create audio manager object
    private AudioManager audioManager;

    private void Start()
    {
        //Access the audio manger object 
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    /// <summary>
    /// Description: This method will check the collisions the enemy knights will interact with. 
    /// If they collide with the catapult ammo or the losescene trigger, they are destroyed.
    /// </summary>
    /// <param name="collision"> collider is the enemy knight </param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "CatapultAmmo")
        {
            enemyLives--;
            healthBarSlider.value = ((float)enemyLives / 2);
            //Play corresponding SFX
            audioManager.PlaySFX(GameObject.FindObjectOfType<AudioManager>().Damage);
            if (enemyLives == 0)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.transform.name == "LoseSceneTrigger")
        {
            Destroy(gameObject);
        }
    }
}
