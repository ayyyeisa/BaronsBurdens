/*****************************************************************************
// File Name : CatapultAmmo.cs
// Author : Tri Nguyen, Ryan Egan
// Creation Date : October 23, 2023
//
// Brief Description : This is a file that works on the behavior and movement 
///                    of the ammo for the catapult when collides with different objects
///                    in the Catapult minigame
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultAmmo : MonoBehaviour
{
    /// <summary>
    /// This function destroys the ammo when they collide with enemies or the ground
    /// </summary>
    /// <param name="collision"> collision between ammo and another game object </param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Destroy(gameObject);
            CatapultMovement.IsAmmoDestroyed = true;
        }
        else if (collision.transform.tag == "Ground")
        {
            Destroy(gameObject);
            CatapultMovement.IsAmmoDestroyed = true;
        }
        
    }

    

}
