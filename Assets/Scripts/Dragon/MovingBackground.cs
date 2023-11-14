/*****************************************************************************
// File Name : MovingBackground.cs
// Author : Ryan Egan
// Creation Date : October 25, 2023
//
// Brief Description :  Description: This file makes the 
                        background in the game move and loop repeatedly
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    [Header("Values for background's movement")]
    [Tooltip("Speed at which background is moving")]
    [SerializeField] private float scrollSpeed;
    [Tooltip("Width of the background. Used to loop background")]
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

    /// <summary>
    /// Description: This method is what makes the background move
    /// </summary>
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

    /// <summary>
    /// Description: This method is what makes the background loop back to its original position
    /// </summary>
    public virtual void HandleOffScreen(ref Vector2 pos)
    {
        pos.x += 2 * scrollWidth;
    }
}
