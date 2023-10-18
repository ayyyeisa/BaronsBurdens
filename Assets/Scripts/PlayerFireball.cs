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
