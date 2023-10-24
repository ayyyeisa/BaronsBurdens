/// <summary>
/// 
/// Author: Ryan Egan, Scott Berry, Tri Nguyen, Carl Crumer, Isa Luluquisin
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

    // Coroutine to spawn an enemy knight every 2 seconds
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

    // enables action map
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

    // spawns in an enemy knight
    public void SpawnEnemyKnight()
    {
        Vector2 playerPause = enemyKnightSpawn.transform.position;
        GameObject temp = Instantiate(enemyKnight, playerPause, Quaternion.identity);

        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
    }

    // spawns in ammo for the catapult
    public void SpawnAmmo()
    {
        if (didShoot)
        {
            Vector2 playerPause = catapultAmmoSpawn.transform.position;
            GameObject temp = Instantiate(catapultAmmo, playerPause, Quaternion.identity);

            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(-7, 0);

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
