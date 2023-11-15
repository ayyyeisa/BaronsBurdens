/*****************************************************************************
// File Name : DragonMovement.cs
// Author : Ryan Egan, Isa Luluquisin
// Creation Date : October 25, 2023
//
// Brief Description :  This is a file that works on most controls for the 
                        Dragon Riding minigame, as well as spawning in player and enemy fireballs
                        and implementing the Game timer
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class DragonMovement : MonoBehaviour
{
    #region variables

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D Rb2D;
    [Tooltip("Speed at which player is able to move up and down screen")]
    [SerializeField] private float moveSpeed;

    [Header("Fireball References")]
    [Tooltip("Player fireball object")]
    [SerializeField] private GameObject pF;
    [Tooltip("Spawning points for enemy fireballs")]
    [SerializeField] private GameObject enemyFireballSpawn;
    [Tooltip("Enemy fireball object")]
    [SerializeField] private GameObject eF;

    [Header("Text Scenes")]
    [SerializeField] private TMP_Text livesText;
    [Tooltip("Text that states the keyboard inputs")]
    [SerializeField] private TMP_Text controlsText;
    [Tooltip("Canvas that provides instruction before game has started")]
    [SerializeField] private GameObject startGameScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    //create audio manager object
    private AudioManager audioManager;

    // ui timer variables
    [Tooltip("Text for the UI timer")]
    [SerializeField] private TMP_Text timerText;
    private float currentTime = 0f;
    private float startingTime = 20f;

    public Coroutine EnemyFireballRef;
    public Coroutine GameTimerRef;

    private bool isGameRunning;
    private bool spaceIsPressed;
    private bool gameIsOver;

    //whether player's fireball was destroyed
    public static bool isFireballDestroyed;

    //input actions
    private InputAction move;
    private InputAction fireball;
    private InputAction restart;
    private InputAction quit;

    //determines whether player dragon is moving
    private bool isMoving;
    //determines whether a fireball was shot and onscreen
    public bool didFire;
    //finds out which directin player was moving (up or down)
    private float moveDirection;
    //numer of times players may be hit before losing
    private int numOfLives = 3;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Access the audio manger object 
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
        // Enables the action map
        EnableInputs();

        // bools are set
        isMoving = false;
        didFire = false;
        isFireballDestroyed = true;
        isGameRunning = false;
        spaceIsPressed = false;
        gameIsOver = false;

        currentTime = startingTime;

    }

    // Update is called once per frame
    void Update()
    {
        if(!(isGameRunning) && !(gameIsOver))
        {
            GameStartCheck();
        }

        if (isMoving)
        {
            moveDirection = move.ReadValue<float>();
        }

        // Starts spawning enemy fireballs while game timer is running
        if (spaceIsPressed && isGameRunning)
        {
            if (EnemyFireballRef == null)
            {
                EnemyFireballRef = StartCoroutine(EnemyFireballTimer());
            }
            currentTime -= 1 * Time.deltaTime;
            int convertTimeToInt = Mathf.CeilToInt(currentTime);
            if (currentTime < 0)
            {
                currentTime = 0;
            }
            timerText.GetComponent<TMP_Text>().text = "Timer: " + convertTimeToInt;
        }


    }

    void FixedUpdate()
    {
        // Giving movement to player dragon
        if (isMoving)
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveDirection * moveSpeed);
        }
        // Stopping movement
        if (!isMoving)
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// This checks the collision the player will be interacting with.
    /// If the player collides with an enemy fireball and has more than 0 lives, they lose a life.
    /// When lives reach 0, player loses.
    /// </summary>
    /// <param name="collision"> collision between player and another game object </param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.name == "Enemy_Fireball(Clone)") && (numOfLives > 0))
        {
            numOfLives--;
            livesText.text = "Lives: " + numOfLives.ToString();
            if(numOfLives == 0)
            {
                loseScreen.gameObject.SetActive(true);
                controlsText.gameObject.SetActive(false);
                livesText.gameObject.SetActive(false);
                isGameRunning = false;
            }
            
        }
    }

    /// <summary>
    /// Description: This method checks if the spacebar was pressed for the first time in the minigame.
    /// If it was, the game timer coroutine will start
    /// </summary>
    private void GameStartCheck()
    {
        if(spaceIsPressed)
        {
            if (GameTimerRef == null)
            {
                GameTimerRef = StartCoroutine(GameTimer());
            }
        }
    }

    #region theCoroutines

    /// <summary>
    /// Description: This function is a coroutine that will spawn in an enemy fireball per second
    /// </summary>
    public IEnumerator EnemyFireballTimer()
    {
       
        yield return new WaitForSeconds(1f);
        SpawnEnemyFireball();
        EnemyFireballRef = null;

    }
    
    /// <summary>
    /// Description: This function is a coroutine that is the main game timer, set to 20 seconds
    /// </summary>
    public IEnumerator GameTimer()
    {
        isGameRunning = true;
        yield return new WaitForSeconds(20f);
        GameTimerRef = null;
        isGameRunning = false;
        gameIsOver = true;

        if(numOfLives > 0)
        {
            winScreen.gameObject.SetActive(true);
            controlsText.gameObject.SetActive(false);
            livesText.gameObject.SetActive(false);
        }
    }
    #endregion


    /// <summary>
    /// Description: This function will enable the action map and read inputs
    /// </summary>
    public void EnableInputs()
    {
        playerInput.currentActionMap.Enable();

        move = playerInput.currentActionMap.FindAction("Move");
        fireball = playerInput.currentActionMap.FindAction("Fireball");
        restart = playerInput.currentActionMap.FindAction("Restart");
        quit = playerInput.currentActionMap.FindAction("Quit");

        move.started += Move_started;
        move.canceled += Move_canceled;
        fireball.started += Fireball_started;
        restart.started += Restart_started;
        quit.started += Quit_started;
    }

    #region inputActions
    private void Fireball_started(InputAction.CallbackContext obj)
    {
        //if space is being pressed for the first time
        if(!spaceIsPressed)
        {
            startGameScreen.gameObject.SetActive(false);
            livesText.gameObject.SetActive(true);
            controlsText.gameObject.SetActive(true);
            livesText.text = "Lives: " + numOfLives;
            spaceIsPressed = true;

        }
        else if(spaceIsPressed)
        {
            //Play corresponding SFX
            audioManager.PlaySFX(GameObject.FindObjectOfType<AudioManager>().Fireball);
            if (isFireballDestroyed && isGameRunning)
            {
                didFire = true;
                SpawnFireball();

                print("Fireball shot");

                isFireballDestroyed = false;

            }
        }

    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        isMoving = false;
    }

    private void Move_started(InputAction.CallbackContext obj)
    {
        //Play corresponding SFX
        audioManager.PlaySFX(GameObject.FindObjectOfType<AudioManager>().WingBeat);
        if (isGameRunning)
        {
            isMoving = true;
        }
        
    }
    private void Quit_started(InputAction.CallbackContext obj)
    {
        // Loads back the Main menu scene
        SceneManager.LoadScene(1);

    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        // loads back the Dragon Riding Scene
        SceneManager.LoadScene(2);
    }
    #endregion

    #region spawnFunctions

    /// <summary>
    /// Description: This function will spawn in the player's fireball upon shooting
    /// </summary>
    public void SpawnFireball()
    {
        if (didFire)
        {
            Vector2 playerPause = gameObject.transform.position;
            playerPause.x += 4.5f;
            playerPause.y -= 1.3f;
            GameObject temp = Instantiate(pF, playerPause, Quaternion.identity);

            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(7, 0);
        }
    }


    /// <summary>
    /// Description: This function is what spawns in the enemy fireballs at a fixed x position and random y position
    /// </summary>
    public void SpawnEnemyFireball()
    {
        if(isGameRunning)
        {
            Vector2 playerPause = new Vector2(enemyFireballSpawn.transform.position.x, Random.Range(-3.55f, 3.55f));
            GameObject temp = Instantiate(eF, playerPause, Quaternion.identity);

            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(-7, 0);
        }
       // Vector2 playerPause = new Vector2(enemyFireballSpawn.transform.position.x, Random.Range(-3.55f, 3.55f));
       // GameObject temp = Instantiate(eF, playerPause, Quaternion.identity);

      //  temp.GetComponent<Rigidbody2D>().velocity = new Vector2(-7, 0);
    }
    #endregion

    public void OnDestroy()
    {
        move.started -= Move_started;
        move.canceled -= Move_canceled;
        fireball.started -= Fireball_started;
        restart.started -= Restart_started;
        quit.started -= Quit_started;
    }
}
