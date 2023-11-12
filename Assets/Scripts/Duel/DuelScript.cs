///<summary>
///PlayerDuel.cs 
///
///Author(s): Carl Crumer, Isa Luluquisin,Scott Berry 
///Creation Date: October 24, 2023
///
///Description: The script the player functions of the game,input, and User Interface
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class DuelScript : MonoBehaviour
{
    #region variables
    //Text for instructions, the player's lives, and a time
    //use this 
    public TMP_Text instructionText;
    public TMP_Text livesText;
    public TMP_Text timerText;
    public TMP_Text hitText;
    public TMP_Text missText;
    [SerializeField] private TMP_Text controlsText;
    private float gameDuration = 20f;
    private float timer = 0f;
    private bool isRunning = false;
    private int lastInstruction = 0;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject startGameScreen;
    [SerializeField] private GameObject winScene;
    [SerializeField] private GameObject loseScene;
    [SerializeField] private GameObject Hit;
    [SerializeField] private GameObject Miss;
    
   // [SerializeField] private AudioSource newInstructionPop;
  
    private InputAction restart;
    private InputAction quit;

    //player gets 4 lives, the game lasts for 20 seconds
    private int lives = 4;
    private int hits = 0;
    private KeyCode action = KeyCode.None;
    
    
    //private int roundCount = 0;

    private KeyCode[] validInputs = { KeyCode.F, KeyCode.A, KeyCode.Space };

    //valuables for animations
    const string ATTACK_ANIM = "AttackSpace";
    const string BLOCK_ANIM = "BlockA";
    const string PARRY_ANIM = "ParryF";
    private Animator duelAnimator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        isRunning = false;
        playerInput.currentActionMap.Disable(); 
        timer = 0f;
        lives = 4;
        Miss.gameObject.SetActive(false);
        Hit.gameObject.SetActive(false);
        Update();
        StartCoroutine(GameLoop());
        duelAnimator = GetComponent<Animator>();
    }
    // The Update function is called once per frame and handles game state updates,
    // track the timer, check for player input, and ends the game
    // when the timer reaches the game duration. It also updates the user interface.
    void Update()
    {

        if(isRunning)
        {
         timer += Time.deltaTime; // Increment the timer
            controlsText.gameObject.SetActive(true);
        }
       

        livesText.text = "Lives: " + lives;
        timerText.text = "Time: " + (int)(gameDuration - timer);
        if (hits == 4)
        {
            EndGame();
           
        }
        // win/lose conditions or time runs out 
        if (lives == 0 || hits == 4 || ((int)(gameDuration - timer)) == 0)
        {
            EndGame();
            
        }

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            duelAnimator.SetTrigger(ATTACK_ANIM);
        }
        else if(Input.GetKeyDown(KeyCode.A)) 
        {
            duelAnimator.SetTrigger(BLOCK_ANIM);
        }
        else if(Input.GetKeyDown(KeyCode.F)) 
        {
            duelAnimator.SetTrigger(PARRY_ANIM);
        }
    }
    //corotutine that handles all game functions. Takes Users input, checks users 
    //input and acts accordinlgy based on if correct key was placed in time, user wants
    //to quit or restart, or if user doesn't input correct key or any key at all
    private IEnumerator GameLoop()
    {
        bool correctKeyEntered = false;
        // Enable the start screen
        startGameScreen.gameObject.SetActive(true);

        // Wait for the player to press the spacebar to start the game
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        // Disable the start screen and start the game
        startGameScreen.gameObject.SetActive(false);
        controlsText.gameObject.SetActive(false);
        isRunning = true;

        // Display "READY! SET! FIGHT!"
        instructionText.text = "READY!";
        yield return new WaitForSeconds(.5f);
        instructionText.text = "SET!";
        yield return new WaitForSeconds(.5f);
        instructionText.text = "FIGHT!";
        yield return new WaitForSeconds(.5f);

        
        // Main game loop
        while (isRunning)
        {
            
       
            // Display action and record start time
            string currentInstruction = GetRandomInstruction();

            // Display the current instruction
            instructionText.text = currentInstruction;
           // newInstructionPop.Play();
            
            correctKeyEntered = false;

            
            float startTime = Time.time;

            // Process player input for 1.5 seconds
            while (Time.time - startTime < 1.5f)
            {
                if (Input.GetKeyDown(action))
                {
                    // Player gets a hit
                    hits++;
                    correctKeyEntered = true;
                    yield return StartCoroutine(HitScreen());
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    QuitGame();
                    yield break;
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    RestartGame();
                    yield break;
                }
                yield return null;
            
            }

            // Player misses after 2 seconds
            if (!correctKeyEntered)
            {
                lives--;
                yield return StartCoroutine(MissScreen());
             
            }
           
        }
    }
    //IEnumerator to display Hit Screen for 1 second 
    
    private IEnumerator HitScreen()
    {
        Hit.gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        Hit.gameObject.SetActive(false);
        yield break;


    }
    //IEnumerator to display Miss Screen for 1 second
    private IEnumerator MissScreen()
    {
       Miss.gameObject.SetActive(true);
       yield return new WaitForSeconds(.1f);
       Miss.gameObject.SetActive(false);
        yield break;

    }
    // The Update function is called once per frame and handles game state updates,
    // track the timer, check for player input, and ends the game
    // when the timer reaches the game duration. It also updates the user interface.

    // changes the isRunning boolean to false and changes instruction
    //text on UI to read win or lose

    //EndGame sets the running boolean to false. And displays winning or losing 
    //screen based on the condition, and then takes the player back to the main menu
    private IEnumerator HoldScreen()
    {
        yield return new WaitForSeconds(3f);
    }

   
    private void EndGame()
    {
        isRunning = false;
        EnableInputs();
        //player wins
        if (hits == 4)
        {
            //display win scene
            winScene.gameObject.SetActive(true);
            HoldScreen();
            hits = 0;
            lives = 4;
            controlsText.gameObject.SetActive(false);
        }
        //loser
        else
        {
            //display losing scene
            loseScene.gameObject.SetActive(true);
            HoldScreen();
            hits = 0;
            lives = 4;
            controlsText.gameObject.SetActive(false);
        }

    }

    //GetRandomInstruction creates a random index in the list of actions and 
    //selects one of the 3 attacks for the Enemy/
    private string GetRandomInstruction()
    {
        string[] instructions = { "Parry (Press F)", "Block (Press A)", "Attack (Press Space)" };
        // int randomIndex = Random.Range(0, instructions.Length);

        // if (randomIndex == 0)
        // {
        //     action = KeyCode.F;
        // }
        // else if (randomIndex == 1)
        // {
        //    action = KeyCode.A;
        // }
        // else if (randomIndex == 2)
        //  {
        //     action = KeyCode.Space;
        //  }
        // return action;
        int randomIndex = Random.Range(0, validInputs.Length);
        if(randomIndex==lastInstruction)
        {
            randomIndex = Random.Range(0, validInputs.Length);
        }

        action = validInputs[randomIndex];
        lastInstruction = randomIndex;
        return instructions[randomIndex];
    }
 //returns the string variable corresponding to the action chosen for the enemy
 //i.e keycode.F for Parry, keycode.A for Block,keycode.space for Attack
 

    //back to main menu
    private void QuitGame()
    {
       SceneManager.LoadScene("MainMenu");
    }
    //back to start of the scene
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void EnableInputs()
    {
        playerInput.currentActionMap.Enable();

        quit = playerInput.currentActionMap.FindAction("Quit");
        restart = playerInput.currentActionMap.FindAction("Restart");

        restart.started += Restart_started;
        quit.started += Quit_started;
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        //reloads current scene when prompted during win or lose scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Quit_started(InputAction.CallbackContext obj)
    {
        //quits game with escape key
        SceneManager.LoadScene("MainMenu");
    }
}

