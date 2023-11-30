
/*****************************************************************************
// File Name : CatapultMovement.cs
// Author : Ryan Egan, Tri Nguyen, Isa Luluquisin
// Creation Date : October 23, 2023
//
// Brief Description : This is a file that works on most controls for the 
/// Catapult minigame, as well as spawning in enemy knights and 
/// ammo for the catapult
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class CatapultMovement : MonoBehaviour
{
    #region variables
    [Tooltip("Point at which the ammo will land")]
    [SerializeField] private GameObject trajectoryPoint;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D Rb2D;

    [Tooltip("Speed at which catapult is moving")]
    [SerializeField] private float moveSpeed;

    [Header("ENEMY KNIGHT REFERENCES")]
    [SerializeField] private GameObject[] EnemyPrefabs;
    [SerializeField] private GameObject enemyKnightSpawn;

    [Header("AMMO REFERENCES")]
    [SerializeField] private GameObject catapultAmmo;
    [SerializeField] private GameObject catapultAmmoSpawn;

    [Header("CANVAS REFERENCES")]
    [Tooltip("What appears to players before game has started")]
    [SerializeField] private GameObject startMinigame;
    [SerializeField] private GameObject winScene;
    [SerializeField] private GameObject loseScene;
    [Tooltip("Text that explains what keyboard inputs are")]
    [SerializeField] private GameObject controlsText;

    [Header("INPUT_ACTIONS")]
    private InputAction move;
    private InputAction shoot;
    private InputAction restart;
    private InputAction quit;

    [Tooltip("Coroutine for enemy knights")]
    public Coroutine EnemyKnightRef;

    [Header("BOOLS")]
    [Tooltip("Was the spacebar pressed")]
    private bool spaceIsPressed;
    private bool gameIsRunning;
    [Tooltip("Is the catapult moving")]
    private bool isMoving;
    [Tooltip("Did player shoot ammo")]
    private bool didShoot;
    [Tooltip("Is player ammo still on screen or destroyed")]
    public static bool IsAmmoDestroyed;

    //create audio manager object
    private AudioManager audioManager;

   
    [Tooltip("Direction which trajectory arrow is moving")]
    private float moveDirection;
    [Tooltip("Number of enemy knights to defeat during game")]
    private int numOfEnemyKnights = 10;

    [Header("UI TIMER VARIABLES")]
    [Tooltip("Number of knights there are left")]
    [SerializeField] private TMP_Text knightCounter;
    [Header("UI timer")]
    [SerializeField] private TMP_Text timerText;
    private float currentTime = 0f;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //Access the audio manger object 
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        // Enabling action map, setting bools
        EnableInputs();
        isMoving = false;
        didShoot = false;
        IsAmmoDestroyed = true;
        spaceIsPressed = false;
        gameIsRunning = false;

        //set start timer to 20
        currentTime = 20f;

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
            Vector2 enemySpawnPos = enemyKnightSpawn.transform.position;
            
            int random = Random.Range(2, 4);
            GameObject temp = Instantiate(EnemyPrefabs[random-2], enemySpawnPos, Quaternion.identity);
            temp.tag = "Enemy";
            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(random, 0);
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
    /// <summary>
    /// This triggers the lose scene if an enemy soldier collides with the castle.
    /// When the lose scene is triggered, the coroutine is also stopped.
    /// </summary>
    /// <param name="collision"> collision between enemy sprite and trigger scene </param>
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
            //Play corresponding SFX
            audioManager.PlaySFX(GameObject.FindObjectOfType<AudioManager>().Catapult);
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
