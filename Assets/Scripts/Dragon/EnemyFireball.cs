/*****************************************************************************
// File Name : EnemyFireball.cs
// Author : Ryan Egan
// Creation Date : October 23, 2023
//
// Brief Description :  This is a file that works on the behavior for the enemy
                        fireballs that spawn in for the Dragon Riding minigame
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : MonoBehaviour
{
    /// <summary>
    /// This function checks the collisions the enemy fireballs will interact with. 
    /// If a fireball collides with the edge of the screen, another fireball,
    /// or the player's dragon, it is destroyed.
    /// </summary>
    /// <param name="collision"> collider is the enemy fireball </param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (collision.transform.name == "Fire_Ball(Clone)")
        {
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "DragonPlayer")
        {
            Destroy(gameObject);
        }
    }
}
