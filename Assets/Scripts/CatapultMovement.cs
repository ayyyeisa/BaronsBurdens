/// <summary>
/// 
/// Author: Ryan Egan, Tri Nguyen
/// Date: October 23, 2023
/// 
/// Description: This is a file that works on most controls for the 
/// Catapult minigame, as well as spawning in enemy knights and 
/// ammo for the catapult
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatapultMovement : MonoBehaviour
{
    #region variables
    [SerializeField] private GameObject trajectoryPoint;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D Rb2D;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject enemyKnight;
    [SerializeField] private GameObject enemyKnightSpawn;
    [SerializeField] private GameObject catapultAmmo;
    [SerializeField] private GameObject catapultAmmoSpawn;

    private InputAction move;
    private InputAction shoot;

    public Coroutine EnemyKnightRef;

    private bool isMoving;
    private bool didShoot;
    private float moveDirection;
    public static bool IsAmmoDestroyed;
    private int numOfEnemyKnights = 20;

    //variables for countdown timer
    private float currentTime = 0f;
    private float startingTime = 20f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Enabling action map, setting bools
        EnableInputs();
        isMoving = false;
        didShoot = false;
        IsAmmoDestroyed = true;

        currentTime = startingTime;

    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        if (currentTime < 0)
        {
            currentTime = 0;
        }
        Debug.Log("Current time is: " + currentTime);
        if (isMoving)
        {
            moveDirection = move.ReadValue<float>();
        }

        // Starts coroutine of spawning enemy knights
        if (numOfEnemyKnights > 0)
        {
            if (EnemyKnightRef == null)
            {
                EnemyKnightRef = StartCoroutine(EnemyKnightTimer());
                if(currentTime == 0 && numOfEnemyKnights > 0)
                {
                    Debug.Log("You lose");
                }
                else if(currentTime == 0 && numOfEnemyKnights== 0)
                {
                    Debug.Log("You win");
                }
            }
        }

    }


    /// <summary>
    /// Description: This function is a coroutine that will spawn in an enemy knight every 2 seconds
    /// </summary>
    public IEnumerator EnemyKnightTimer()
    {
        SpawnEnemyKnight();
        yield return new WaitForSeconds(2f);
        EnemyKnightRef = null;
        numOfEnemyKnights--;

    }

    void FixedUpdate()
    {
        // giving movement to trajectory arrow
        if (isMoving)
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection * moveSpeed, 0);
        }
        // stopping movement
        if (!isMoving)
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    /// <summary>
    /// Description: This function will enable the action map and read inputs
    /// </summary>
    public void EnableInputs()
    {
        playerInput.currentActionMap.Enable();

        move = playerInput.currentActionMap.FindAction("Move");
        shoot = playerInput.currentActionMap.FindAction("Shoot");

        move.started += Move_started;
        move.canceled += Move_canceled;
        shoot.started += Shoot_started;
    }

    #region spawnFunctions


    /// <summary>
    /// Description: This function is what spawns in an enemy knight at its spawn point
    /// </summary>
    public void SpawnEnemyKnight()
    {
        Vector2 playerPause = enemyKnightSpawn.transform.position;
        GameObject temp = Instantiate(enemyKnight, playerPause, Quaternion.identity);
        temp.transform.tag = "Enemy";
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(1,4), 0);
    }

    /// <summary>
    /// Description: This function is what spawns in the catapult ammo at its spawn point
    /// </summary>
    public void SpawnAmmo()
    {
        if (didShoot)
        {
            Vector2 playerPause = catapultAmmoSpawn.transform.position;
            GameObject temp = Instantiate(catapultAmmo, playerPause, Quaternion.identity);
            temp.transform.tag = "CatapultAmmo";
            //calculate distance between ammo spawn point and arrow pos
            float targetDistance = temp.transform.position.x - trajectoryPoint.transform.position.x;

            //calculate velocity needed to reach the trajectory point
            float initialVelocity = Mathf.Sqrt(targetDistance * Physics2D.gravity.magnitude / Mathf.Sin(90 * Mathf.Deg2Rad));

            //Calculate vertical and horizontal speed
            float horizontalSpeed = -initialVelocity * Mathf.Cos(45 * Mathf.Deg2Rad) *2;
            float verticalSpeed = initialVelocity * Mathf.Sin(45 *Mathf.Deg2Rad) *2;

            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, verticalSpeed);
            didShoot = false;
            IsAmmoDestroyed = false;
        }
    }
    #endregion

    #region inputActions
    private void Shoot_started(InputAction.CallbackContext obj)
    {
        if (IsAmmoDestroyed)
        {
            print("Shooting with catapult");
            didShoot = true;

            SpawnAmmo();
        }

    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        isMoving = false;
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        isMoving = true;
    }
    #endregion

    public void OnDestroy()
    {
        move.started -= Move_started;
        move.canceled -= Move_canceled;
        shoot.started -= Shoot_started;
    }
}
