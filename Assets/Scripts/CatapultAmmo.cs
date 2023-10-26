/// <summary>
/// 
/// Author: Ryan Egan, Tri Nguyen
/// October 23, 2023
/// 
/// Description: This is a file that works on the behavior and movement 
/// of the ammo for the catapult in the Catapult minigame
/// 
/// </summary>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultAmmo : MonoBehaviour

{
  
    

    // Update is called once per frame
   
    /// <summary>
    /// Description: This method will check the collisions the catapult ammo will interact with
    /// </summary>
    /// <param>
    /// Collision2D collision
    /// Collider is the catapult ammo
    /// </param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "EnemyKnight(Clone)")
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
