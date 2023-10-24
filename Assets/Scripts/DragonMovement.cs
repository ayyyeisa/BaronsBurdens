/// <summary>
/// 
/// Author: Ryan Egan
/// Date: October 23, 2023
/// 
/// Description: This is a file that works on most controls for the 
/// Dragon Riding minigame, as well as spawning in player and enemy fireballs
/// and implementing the Game timer
/// 
/// </summary>


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
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject pF;
    [SerializeField] private GameObject enemyFireballSpawn;
    [SerializeField] private GameObject eF;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject startGameScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    


    public Coroutine EnemyFireballRef;
    public Coroutine GameTimerRef;
    private bool isGameRunning;
    private bool spaceIsPressed;
    private bool gameIsOver;

    public static bool isFireballDestroyed;

    private InputAction move;
    private InputAction fireball;
    private InputAction restart;
    private InputAction quit;

    private bool isMoving;
    public bool didFire;
    private float moveDirection;
    private int numOfLives = 3;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Enables the action map

        EnableInputs();

        // bools are set

        isMoving = false;
        didFire = false;
        isFireballDestroyed = true;
        isGameRunning = false;
        spaceIsPressed = false;
        gameIsOver = false;

        

        // Starting game timer
        
      /*  if (GameTimerRef == null)
        {
            GameTimerRef = StartCoroutine(GameTimer());
        }
      */


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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.name == "EnemyFireball(Clone)") && (numOfLives == 0))
        {
            // Destroy(gameObject);
           // loseScreen.gameObject.SetActive(true);
        }
        else if ((collision.transform.name == "EnemyFireball(Clone)") && (numOfLives > 0))
        {
            numOfLives--;
            livesText.text = "Lives: " + numOfLives.ToString();
            if(numOfLives == 0)
            {
                loseScreen.gameObject.SetActive(true);
            }
            
        }
    }

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
    // Spawns enemy fireballs per second
    public IEnumerator EnemyFireballTimer()
    {
       // SpawnEnemyFireball();
        yield return new WaitForSeconds(1f);
        SpawnEnemyFireball();
        EnemyFireballRef = null;

    }
    
    // main game timer, set to 20 seconds
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
        }
    }
    #endregion

    // enables action map
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

    private void Quit_started(InputAction.CallbackContext obj)
    {
        // this will be another SceneManager.LoadScene() method for when the main menu scene is complete
        Application.Quit();
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(1);
    }

    #region inputActions
    private void Fireball_started(InputAction.CallbackContext obj)
    {
        if(!spaceIsPressed)
        {
            startGameScreen.gameObject.SetActive(false);
            livesText.gameObject.SetActive(true);
            livesText.text = "Lives: " + numOfLives;
            spaceIsPressed = true;

        }
        else if(spaceIsPressed)
        {
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
        if(isGameRunning)
        {
            isMoving = true;
        }
        
    }
    #endregion

    #region spawnFunctions
    public void SpawnFireball()
    {
        if (didFire)
        {
            Vector2 playerPause = gameObject.transform.position;
            playerPause.x += 5.0f;
            playerPause.y -= 1.0f;
            GameObject temp = Instantiate(pF, playerPause, Quaternion.identity);

            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(7, 0);
        }
    }

    public void SpawnEnemyFireball()
    {
        Vector2 playerPause = new Vector2(enemyFireballSpawn.transform.position.x, Random.Range(-3.55f, 3.55f));
        GameObject temp = Instantiate(eF, playerPause, Quaternion.identity);

        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(-7, 0);
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
