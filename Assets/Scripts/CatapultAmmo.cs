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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Debug.Log("enemy hit");
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
