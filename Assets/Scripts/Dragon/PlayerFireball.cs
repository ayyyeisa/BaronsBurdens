/*****************************************************************************
// File Name : PlayerFireball.cs
// Author : Ryan Egan
// Creation Date : October 23, 2023
//
// Brief Description : This is a file that works on the behavior of 
                       the player fireballs that spawn in for the Dragon Riding
                       minigame
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : MonoBehaviour
{
    /// <summary>
    /// This method will check the collisions the player fireball will interact with. 
    /// If a player's fireballs collide with the edge of the screen or an enemy's fireball,
    /// they will be destroyed.
    /// </summary>
    /// <param name="collision"> collider is the player fireball </param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            Destroy(gameObject);
            DragonMovement.isFireballDestroyed = true;
        }
        else if (collision.transform.name == "Enemy_Fireball(Clone)")
        {
            Destroy(gameObject);
            DragonMovement.isFireballDestroyed = true;
        }
    }
}
