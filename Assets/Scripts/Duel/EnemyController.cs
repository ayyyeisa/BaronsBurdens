/*****************************************************************************
// File Name : EnemyController.cs
// Author : Carl Crumer
// Creation Date : 11/29/2023
//
// Brief Description : Handles the animations and actions of the enemy character
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Constants for animation names
    const string ENEMY_BLOCK_ANIM = "EnemyBlock";
    const string ENEMY_ATTACK_ANIM = "EnemyAttack";
    const string ENEMY_PARRY_ANIM = "EnemyParry";

    // Reference to the Animator component
    private Animator duelAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the same GameObject
        duelAnimator = GetComponent<Animator>();
    }

    // Triggers the animation for enemy parry
    public void StartEnemyParry()
    {
        duelAnimator.SetTrigger(ENEMY_PARRY_ANIM);
    }

    // Triggers the animation for enemy block
    public void StartEnemyBlock()
    {
        duelAnimator.SetTrigger(ENEMY_BLOCK_ANIM);
    }

    // Triggers the animation for enemy attack
    public void StartEnemyAttack()
    {
        duelAnimator.SetTrigger(ENEMY_ATTACK_ANIM);
    }
}


