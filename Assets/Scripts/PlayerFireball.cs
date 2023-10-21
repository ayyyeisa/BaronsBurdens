/// <summary>
/// 
/// Author: Ryan Egan, Scott Berry, Tri Nguyen, Carl Crumer, Isa Luluquisin
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

    // Checking collisions with player fireball
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            Destroy(gameObject);
            DragonMovement.isFireballDestroyed = true;
        }
        else if (collision.transform.name == "EnemyFireball(Clone)")
        {
            Destroy(gameObject);
            DragonMovement.isFireballDestroyed = true;
        }
    }
}
