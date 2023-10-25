/// <summary>
/// 
/// Author: Ryan Egan, Scott Berry, Tri Nguyen, Carl Crumer, Isa Luluquisin
/// 
/// Description: This is a file that works on the behavior for the enemy
/// fireballs that spawn in for the Dragon Riding minigame
/// 
/// </summary>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // checking collisions with enemy fireballs
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (collision.transform.name == "PlayerFireball(Clone)")
        {
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "DragonPlayer")
        {
            Destroy(gameObject);
        }
    }
}
