/*****************************************************************************
// File Name : CatapultAnim.cs
// Author : Ryan Egan
// Creation Date : October 25, 2023
//
// Brief Description :  This is a file that works on the animation for the catapult
// in the Catapult minigame
*****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultAnim : MonoBehaviour
{
    //catapult animation value
    const string SHOOT_ANIM = "Shoot";
    private Animator catapultAnimator;
    // Start is called before the first frame update
    void Start()
    {
        catapultAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            catapultAnimator.SetTrigger(SHOOT_ANIM);
        }
    }
}
