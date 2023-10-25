/// <summary>
/// 
/// Author: Ryan Egan
/// Date: October 23, 2023
/// 
/// Description: This is a file that works on the behavior of 
/// the player fireballs that spawn in for the Dragon Riding
/// minigame
/// 
/// </summary>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Description: This method will check the collisions the player fireball will interact with
    /// </summary>
    /// <param>
    /// Collision2D collision
    /// Collider is the player fireball
    /// </param>
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
