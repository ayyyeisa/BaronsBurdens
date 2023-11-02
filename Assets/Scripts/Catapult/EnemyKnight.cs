/// <summary>
/// 
/// Author: Ryan Egan, Tri Nguyen
/// Date: October 23, 2023
/// Description: This is a file that works on the behavior for the enemy
/// knights that spawn in for the Catapult minigame
/// 
/// </summary>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnight : MonoBehaviour
{
    /// <summary>
    /// Description: This method will check the collisions the enemy knights will interact with
    /// </summary>
    /// <param>
    /// Collision2D collision
    /// Collider is the enemy knight
    /// </param>
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
