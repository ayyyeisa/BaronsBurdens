using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{

    [SerializeField] private float scrollSpeed;
    [SerializeField] private float scrollWidth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBackground();
    }

    public void MoveBackground()
    {
        Vector2 currentPosition = transform.position;

        currentPosition.x -= scrollSpeed * Time.deltaTime;

        if (currentPosition.x < -scrollWidth)
        {
            // do 
            HandleOffScreen(ref currentPosition);
        }

        transform.position = currentPosition;
    }

    public virtual void HandleOffScreen(ref Vector2 pos)
    {
        pos.x += 2 * scrollWidth;
    }
}
