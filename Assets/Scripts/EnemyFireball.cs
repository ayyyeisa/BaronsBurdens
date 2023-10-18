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
        else if (collision.transform.name == "DragonPlayer")
        {
            Destroy(gameObject);
        }
    }
}
