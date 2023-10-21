using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultAmmo : MonoBehaviour
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

    public Vector2[] Plot(Rigidbody2D rb2D, Vector2 pos, Vector2 force, int steps)
    {
        //create array of steps to draw trajectory line
        Vector2[] result = new Vector2[steps];

        //calculate timestep
        float timeStep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAcceleration = Physics2D.gravity * rb2D.gravityScale * timeStep * timeStep;

        float drag = 1f - timeStep * rb2D.drag;
        Vector2 moveStep = force * timeStep;

        //for every step, add gravity acceleration then multiply by drag force to moveStep then add to current position
        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAcceleration;
            moveStep *= drag;
            pos += moveStep;
            //replace elements in initial result
            result[i] = pos;
        }
        return result;
    }
}
