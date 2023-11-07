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

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject startGameScreen;
    [SerializeField] private GameObject winScene;
    [SerializeField] private GameObject loseScene;
    [SerializeField] private GameObject Hit;
    [SerializeField] private GameObject Miss;
  
    private InputAction restart;
    private InputAction quit;

    //player gets 2 lives, the game lasts for 20 seconds
    private int lives = 2;
    private int hits = 0;
    private KeyCode action = KeyCode.None;
    
    //private int roundCount = 0;

    private KeyCode[] validInputs = { KeyCode.F, KeyCode.A, KeyCode.Space };

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        isRunning = false;
        playerInput.currentActionMap.Disable(); 
        timer = 0f;
        lives = 2;
        Update();
        StartCoroutine(GameLoop());

    }
    // The Update function is called once per frame and handles game state updates,
    // track the timer, check for player input, and ends the game
    // when the timer reaches the game duration. It also updates the user interface.
    void Update()
    {
        livesText.text = "Lives: " + lives;
        timerText.text = "Time: " + (int)(gameDuration - timer);
        
        // win/lose conditions or time runs out 
        if (lives == 0 || hits == 2 || ((int)(gameDuration - timer)) == 0)
        {
            EndGame();
        }

    }
   //corotutine that handles all game functions. Takes Users input, checks users 
   //input and acts accordinlgy based on if correct key was placed in time, user wants
   //to quit or restart, or if user doesn't input correct key or any key at all
    private IEnumerator GameLoop()
    {
        //enabling for game object for start screen
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            startGameScreen.gameObject.SetActive(true);
            yield return null;
        }
        isRunning = true;
        startGameScreen.gameObject.SetActive(false);
        controlsText.gameObject.SetActive(true);
        //ready set fight text
        instructionText.text = "READY!";
        yield return new WaitForSeconds(1f);
        instructionText.text = "SET!";
        yield return new WaitForSeconds(1f);
        instructionText.text = "FIGHT!";
        yield return new WaitForSeconds(1f);

        //while the game is running
        while (isRunning)
        {
            //display action 
            instructionText.text = GetRandomInstruction();
            //start time to capture beginning of action
            float startTime = Time.time;
            //for 2 seconds
            while(Time.time-startTime <2f)
            {
                //if key pressed== current action player gets a hit
                if (Input.GetKeyDown(action))
                { 
                    //instruction text displays correct, Hit! is displayed on screen
                    //player gets hit counted
                    instructionText.text = "CORRECT!";
                    HitScreen();
                    hits++;
                    break;;     
                }
               //quit 
               if (Input.GetKeyDown(KeyCode.Escape))
                {
                  QuitGame();
                   yield break;
                }
                //restart 
               if (Input.GetKeyDown(KeyCode.R))
               {
                  RestartGame();
                   yield break;
                }
                yield return null;
            }
            //player misses out after 2 seconds..(should be changed for beta)
            if (!Input.GetKeyDown(action))
            {
                //player loses a life and MISSED! is displayed
                lives--;
                MissScreen();
                instructionText.text = "MISSED!";
            }
        }
        timer += 2f;
        Update();
            //win and lose conditions
            if (timer >= gameDuration || lives == 0 || hits == 2) 
             {
                isRunning = false;
                EndGame();
            }
            yield return null;
        }
    //IEnumerator to display Hit Screen for 1 second 
    private IEnumerator HitScreen()
    {
        Hit.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        Hit.gameObject.SetActive(false);


    }
    //IEnumerator to display Miss Screen for 1 second
    private IEnumerator MissScreen()
    {
       Miss.gameObject.SetActive(true);
       yield return new WaitForSeconds(1f);
       Miss.gameObject.SetActive(false);

    }
    // The Update function is called once per frame and handles game state updates,
    // track the timer, check for player input, and ends the game
    // when the timer reaches the game duration. It also updates the user interface.

    // changes the isRunning boolean to false and changes instruction
    //text on UI to read win or lose
    
    //EndGame sets the running boolean to false. And displays winning or losing 
    //screen based on the condition, and then takes the player back to the main menu

    private void EndGame()
    {
        isRunning = false;
        EnableInputs();
        //player wins
        if (hits == 2)
        {
            //display win scene
            winScene.gameObject.SetActive(true);
            controlsText.gameObject.SetActive(false);
        }
        //loser
        else
        {
            //display losing scene
            loseScene.gameObject.SetActive(true);
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

        action = validInputs[randomIndex];
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

