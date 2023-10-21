/// <summary>
/// 
/// Author: Ryan Egan, Scott Berry, Tri Nguyen, Carl Crumer, Isa Luluquisin
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Checking collisions with catapult ammo
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
