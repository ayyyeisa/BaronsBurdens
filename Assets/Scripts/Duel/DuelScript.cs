///<summary>
///PlayerDuel.cs 
///
///Author(s): Carl Crumer, Isa Luluquisin,Scott Berry 
///Creation Date: October 24, 2023
///
///Description: The script the player functions of the game,input,timer, and User Interface displaying intructions and lives
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
    public TMP_Text PlayerHP;
    public TMP_Text timerText;
    public TMP_Text hitText;
    public TMP_Text missText;
    public TMP_Text EnemyHP;
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
    private int lives = 5;
    private int hits = 0;
    private int enemyHits = 0;
    private int enemyLives = 5;
    private KeyCode action = KeyCode.None;
    
    
    //private int roundCount = 0;

    private KeyCode[] validInputs = { KeyCode.F, KeyCode.A, KeyCode.Space };

    //valuables for animations
    const string ATTACK_ANIM = "AttackSpace";
    const string BLOCK_ANIM = "BlockA";
    const string PARRY_ANIM = "ParryF";

    const string ENEMY_BLOCK_ANIM = "EnemyBlock";
    const string ENEMY_ATTACK_ANIM = "EnemyAttack";
    const string ENEMY_PARRY_ANIM = "EnemyParry";
    private Animator duelAnimator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        isRunning = false;
        //playerInput.currentActionMap.Disable(); 
        timer = 0f;
        lives = 5;
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
        //if game is running starts the timer
        if(isRunning)
        {
         timer += Time.deltaTime; // Increment the timer
            controlsText.gameObject.SetActive(true);
        }
        //if escape is called the game ends
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
            
        }
        //if R is called the game restarts 
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        //displays player and enemy lives
        //displays timer text
        PlayerHP.text = "Player:"+hits;
        timerText.text = "Time: " + (int)(gameDuration - timer);
        EnemyHP.text = "Enemy:"+enemyHits;
        
        // win/lose conditions or time runs out 
        if (lives == 0 || hits == 5 || ((int)(gameDuration - timer)) == 0)
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
      
        // Display "READY! SET! FIGHT!"
        instructionText.text = "READY!";
        yield return new WaitForSeconds(.5f);
        instructionText.text = "SET!";
        yield return new WaitForSeconds(.5f);
        instructionText.text = "FIGHT!";
        yield return new WaitForSeconds(.5f);
        isRunning = true;

        
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

            // Process player input for .75 of a second
            while (Time.time - startTime < .55f)
            {
                //if key clicked is the same as the action
                if (Input.GetKeyDown(action))
                {
                    //correctly attacked
                  
                    if(action==KeyCode.Space)
                    { 
                        //player gets a hit, and enemy loses a life
                        //coroutine that displays hit screen displays for a second
                        hits++;
                        enemyLives--;
                        //Scripts for enemy animations
                        if(currentInstruction == "Space")
                        {
                            duelAnimator.SetTrigger(ENEMY_BLOCK_ANIM);
                        }
                        else if(currentInstruction == "A") 
                        {
                            duelAnimator.SetTrigger(ENEMY_ATTACK_ANIM);
                        }
                        else if (currentInstruction == "F")
                        {
                            duelAnimator.SetTrigger(ENEMY_PARRY_ANIM);
                        }
                        yield return StartCoroutine(HitScreen());

                    }
                    //correct key was entered
                    correctKeyEntered = true;
                    
                    break;
                }
  
                yield return null;
            
            }

            // Player misses key
            if (!correctKeyEntered)
            {
               //player misses a block 
                if(action==KeyCode.A)
                {
                    //loses a life
                    //the miss screen displays for one second
                    lives--;
                    enemyHits++;
                    //comment these out if you going to fix the scene later on
                    if (currentInstruction == "Space")
                    {
                        duelAnimator.SetTrigger(ENEMY_BLOCK_ANIM);
                    }
                    else if (currentInstruction == "A")
                    {
                        duelAnimator.SetTrigger(ENEMY_ATTACK_ANIM);
                    }
                    else if (currentInstruction == "F")
                    {
                        duelAnimator.SetTrigger(ENEMY_PARRY_ANIM);
                    }
                    yield return StartCoroutine(MissScreen());
                }
               
             
            }
           
        }
    }
   
    
    //IEnumerator to display Hit Screen(player scores) for .1 second 
    //sets the screen active for .1 second then makes 
    //the screen unactive again
    private IEnumerator HitScreen()
    {
        Hit.gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        Hit.gameObject.SetActive(false);
        yield break;


    }
    //IEnumerator to display Miss Screen(player gets attacked) for .1 second 
    //sets the screen active for .1 second then makes 
    //the screen unactive again
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

    //holds the game for 3 seconds
    private IEnumerator HoldScreen()
    {
        yield return new WaitForSeconds(3f);
    }

    //EndGame sets the running boolean to false. And displays winning or losing 
    //screen based on the condition, and then takes the player back to the main menu
    private void EndGame()
    {
        isRunning = false;
        EnableInputs();
        //player wins
        if (hits == 5)
        {
            //display win scene
            winScene.gameObject.SetActive(true);
            HoldScreen();
            hits = 0;
            lives = 5;
            controlsText.gameObject.SetActive(false);
        }
        //loser
        else
        {
            //display losing scene
            loseScene.gameObject.SetActive(true);
            HoldScreen();
            hits = 0;
            lives = 5;
            controlsText.gameObject.SetActive(false);
        }

    }

    //GetRandomInstruction creates a random index in the list of actions and 
    //selects one of the 3 attacks for the Enemy to throw at the player 
    private string GetRandomInstruction()
    {
        //instructions that can be displayed
        string[] instructions = { "F", "A", "Space" };
       //random index in the list of keycodes
        int randomIndex = Random.Range(0, validInputs.Length);
        //assuring that the same instructions aren't being displayed over and over again
        if(randomIndex==lastInstruction)
        {
            randomIndex = Random.Range(0, validInputs.Length);
        }
        //saving current action and index as the last action chosen
        action = validInputs[randomIndex];
        lastInstruction = randomIndex;
        return instructions[randomIndex];
    }
    //returns the string variable corresponding to the action chosen for the enemy
    //i.e keycode.F for Parry, keycode.A for Block,keycode.space for Attack
 

    //loads the main menu
    private void QuitGame()
    {
       SceneManager.LoadScene("MainMenu");
    }
    //restarts the game
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //enables player inputs and turns on 
    //the duel action map
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

