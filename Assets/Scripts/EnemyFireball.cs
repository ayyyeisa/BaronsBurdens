/// <summary>
/// 
/// Author: Ryan Egan
/// Date: October 23, 2023
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

    /// <summary>
    /// Description: This function checks the collisions the enemy fireballs will interact with
    /// </summary>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (collision.transform.name == "Fire_Ball(Clone)")
        {
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "DragonPlayer")
        {
            Destroy(gameObject);
        }
    }
}
