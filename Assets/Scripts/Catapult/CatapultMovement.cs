/// <summary>
/// 
/// Author: Ryan Egan, Tri Nguyen, Isa Luluquisin
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
using TMPro;

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
    [SerializeField] private GameObject startMinigame;
    [SerializeField] private GameObject winScene;
    [SerializeField] private GameObject loseScene;
    [SerializeField] private TMP_Text controlsText;

    private InputAction move;
    private InputAction shoot;
    private InputAction restart;
    private InputAction quit;
    private InputAction skipToWin;

    public Coroutine EnemyKnightRef;

    private bool spaceIsPressed;
    private bool gameIsRunning;

    //valuables for moving enemies
    private bool isMoving;
    private bool didShoot;
    private float moveDirection;
    public static bool IsAmmoDestroyed;
    private int numOfEnemyKnights = 10;
    //variables for countdown timer
    [SerializeField] private TMP_Text knightCounter;
    [SerializeField] private TMP_Text timerText;
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
        spaceIsPressed = false;
        gameIsRunning = false;

        //set start timer to 20
        currentTime = startingTime;

        //basic settings when player starts game
        startMinigame.SetActive(true);
        winScene.SetActive(false);
        loseScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameIsRunning)
        {
            GameStartCheck();
        }

        if (isMoving)
        {
            moveDirection = move.ReadValue<float>();
        }

        if(spaceIsPressed && gameIsRunning)
        {
            //start counting down and display
            currentTime -= 1 * Time.deltaTime;
            int convertTimeToInt = Mathf.CeilToInt(currentTime);
            if (currentTime < 0)
            {
                currentTime = 0;
            }
            timerText.GetComponent<TMP_Text>().text = "Timer: " + convertTimeToInt;
            knightCounter.GetComponent<TMP_Text>().text = "Knights left: " + numOfEnemyKnights;

            // Starts coroutine of spawning enemy knights
            if (numOfEnemyKnights > 0)
            {
                if (EnemyKnightRef == null)
                {
                    EnemyKnightRef = StartCoroutine(EnemyKnightTimer());
                    
                }
            }
            //win scene and lose scene trigger conditions
            if (numOfEnemyKnights == 0)
            {
                winScene.SetActive(true);
                gameIsRunning = false;
                Time.timeScale = 0;
                controlsText.gameObject.SetActive(false);
            }

        }
    }


    /// <summary>
    /// Description: This function is a coroutine that will spawn in an enemy knight every 2 seconds
    /// </summary>
    public IEnumerator EnemyKnightTimer()
    {
        gameIsRunning = true;
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
    /// Description: This method checks if the spacebar was pressed for the first time in the minigame.
    /// If it was, the game timer coroutine will start
    /// </summary>
    private void GameStartCheck()
    {
        if (spaceIsPressed)
        {
            if (EnemyKnightRef == null)
            {
                EnemyKnightRef = StartCoroutine(EnemyKnightTimer());
            }
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
        SceneManager.LoadScene(1);
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }

    #region spawnFunctions


    /// <summary>
    /// Description: This function is what spawns in an enemy knight at its spawn point
    /// </summary>
    public void SpawnEnemyKnight()
    {
        if (gameIsRunning)
        {
            Vector2 playerPause = enemyKnightSpawn.transform.position;
            GameObject temp = Instantiate(enemyKnight, playerPause, Quaternion.identity);
            temp.transform.tag = "Enemy";

            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(2, 4), 0);
        }
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

    #region Collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            loseScene.SetActive(true);
            gameIsRunning = false;
            controlsText.gameObject.SetActive(false);
            StopCoroutine("EnemyKnightTimer");
            Time.timeScale = 0;
        }
    }
    #endregion

    #region inputActions
    private void Shoot_started(InputAction.CallbackContext obj)
    {
        if(!spaceIsPressed)
        {
            startMinigame.gameObject.SetActive(false);
            spaceIsPressed = true;
            controlsText.gameObject.SetActive(true);
        }
        else if(spaceIsPressed)
        {
            if (IsAmmoDestroyed)
            {
                didShoot = true;

                SpawnAmmo();
            }
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
