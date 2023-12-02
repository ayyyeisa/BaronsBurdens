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
    [Header("ANIMATION_CONSTANTS")]
    const string ENEMY_BLOCK_ANIM = "EnemyBlock";
    const string ENEMY_ATTACK_ANIM = "EnemyAttack";
    const string ENEMY_PARRY_ANIM = "EnemyParry";

    [Tooltip("Animator reference")]
    private Animator duelAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the same GameObject
        duelAnimator = GetComponent<Animator>();
    }

    
    /// <summary>
    /// Description: This is a function that will initiate the enemy to parry. Plays the parry animation
    /// </summary>
    public void StartEnemyParry()
    {
        duelAnimator.SetTrigger(ENEMY_PARRY_ANIM);
    }

    
    /// <summary>
    /// Description: This is a function that will initiate the enemy to block. Plays the block animation
    /// </summary>
    public void StartEnemyBlock()
    {
        duelAnimator.SetTrigger(ENEMY_BLOCK_ANIM);
    }

    
    /// <summary>
    /// Description: This is a function that will initiate the enemy to attack. Plays the attack animation
    /// </summary>
    public void StartEnemyAttack()
    {
        duelAnimator.SetTrigger(ENEMY_ATTACK_ANIM);
    }
}


