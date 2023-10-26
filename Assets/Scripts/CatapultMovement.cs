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
using UnityEngine.SceneManagement;

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
    private InputAction restart;
    private InputAction quit;

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

                currentTime -= 1 * Time.deltaTime;
                Debug.Log("Current time: " + currentTime);
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
        restart = playerInput.currentActionMap.FindAction("Restart");
        quit = playerInput.currentActionMap.FindAction("Quit");

        move.started += Move_started;
        move.canceled += Move_canceled;
        shoot.started += Shoot_started;
        restart.started += Restart_started;
        quit.started += Quit_started;
    }

    private void Quit_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(2);
    }

    #region spawnFunctions


    /// <summary>
    /// Description: This function is what spawns in an enemy knight at its spawn point
    /// </summary>
    public void SpawnEnemyKnight()
    {
        Vector2 playerPause = enemyKnightSpawn.transform.position;
        GameObject temp = Instantiate(enemyKnight, playerPause, Quaternion.identity);

        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
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

            Debug.Log("Traj y pos is: " + trajectoryPoint.transform.position.y);
            Debug.Log("Traj x pos is: " + trajectoryPoint.transform.position.x);
            Debug.Log("spawn y pos is: " + temp.transform.position.y);
            Debug.Log("Spawn x pos is: " + temp.transform.position.x);

            float targetDistance = temp.transform.position.x - trajectoryPoint.transform.position.x;
            Debug.Log("target distance is: " + targetDistance);
            float targetHeight = temp.transform.position.y - trajectoryPoint.transform.position.y;
            Debug.Log("target height is: " + targetHeight);
            //calculate velocity needed to reach the trajectory point
            float initialVelocity = targetDistance / (Mathf.Cos(45 * Mathf.Deg2Rad) * (targetDistance / Mathf.Sqrt(2 * Physics2D.gravity.magnitude * targetHeight)));
            Debug.Log("Current Vel is: " + initialVelocity);
            //Calculate vertical and horizontal speed
            float horizontalSpeed = -initialVelocity * Mathf.Sin(45 * Mathf.Deg2Rad);
            float verticalSpeed = initialVelocity * Mathf.Cos(45 *Mathf.Deg2Rad);

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
        restart.started -= Restart_started;
        quit.started -= Quit_started;
    }
}
