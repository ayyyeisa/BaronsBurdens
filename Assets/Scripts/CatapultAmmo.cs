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
<<<<<<< Updated upstream
{
    public float catapultStrength;

    private Rigidbody2D rb2D;
    private LineRenderer lineRenderer;
    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //get start and end position when player drag
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 force = (dragStartPos - dragEndPos) * catapultStrength;

            Vector2[] trajectory = Plot(rb2D, transform.position, force, 500);
            lineRenderer.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];

            }

            lineRenderer.SetPositions(positions);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //pos of the mouse when player release
            dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 force = dragStartPos - dragEndPos;
            rb2D.AddForce(force * catapultStrength);
        }
    }
    /// <summary>
    /// Description: This method will check the collisions the catapult ammo will interact with
    /// </summary>
=======
{ 
    // Checking collisions with catapult ammo
>>>>>>> Stashed changes
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
