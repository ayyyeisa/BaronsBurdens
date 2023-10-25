/// <summary>
/// 
/// Author: Ryan Egan
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Description: This method will check the collisions the enemy knights will interact with
    /// </summary>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "CatapultAmmo(Clone)")
        {
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Wall")
        {
            Destroy(gameObject);
        }


    }
}
