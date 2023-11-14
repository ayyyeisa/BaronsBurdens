/*****************************************************************************
// File Name : EnemyKnightt.cs
// Author : Tri Nguyen, Ryan Egan
// Creation Date : October 23, 2023
//
// Brief Description : This is a file that works on the behavior for the enemy
/// knights that spawn in for the Catapult minigame
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnight : MonoBehaviour
{
    /// <summary>
    /// Description: This method will check the collisions the enemy knights will interact with. 
    /// If they collide with the catapult ammo or the losescene trigger, they are destroyed.
    /// </summary>
    /// <param name="collision"> collider is the enemy knight </param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "CatapultAmmo")
        {
            Destroy(gameObject);
        }
        else if(collision.transform.name == "LoseSceneTrigger")
        {
            Destroy(gameObject);
        }
    }
}
