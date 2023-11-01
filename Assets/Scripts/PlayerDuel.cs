///<summary>
///PlayerDuel.cs 
///
///Author(s): Carl Crumer, Isa Luluquisin
///Creation Date: October 24, 2023
///
///Description: The script the player functions of the game,input, and User Interface
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PlayerDuel : MonoBehaviour
{
    #region variables
    //Text for instructions, the player's lives, and a time
    //use this 
    public TMP_Text instructionText;
    public TMP_Text livesText;
    public TMP_Text timerText;
    private float gameDuration = 20f;
    private float timer = 0f;
    private bool isRunning = false;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject startGameScreen;
    [SerializeField] private GameObject winScene;
    [SerializeField] private GameObject loseScene;
    //private InputAction parry;
    //private InputAction block;
    private InputAction attack;
    private InputAction restart;
    private InputAction quit;
    private bool spaceIsPressed;

    //player gets 2 lives, the game lasts for 20 seconds
    private int lives = 2;
    private int hits = 0;
    
    
    private KeyCode action = KeyCode.None;
    private int roundCount = 0;

    private KeyCode[] validInputs = { KeyCode.F, KeyCode.A, KeyCode.Space };

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Enables action map
        EnableInputs();

        spaceIsPressed = false;
        isRunning = false;

        timer = 0f;
        lives = 2;

        UpdateScreen();
    }
    //Updates the user interface,updates the player's lives and time remaining
    private void UpdateScreen()
    {
        livesText.text = "Lives: " + lives;
        timerText.text = "Time: " + (int)(gameDuration - timer);
    }
   
        // The Update function is called once per frame and handles game state updates,
        // track the timer, check for player input, and ends the game
        // when the timer reaches the game duration. It also updates the user interface.
        void Update()
        {
            if(!isRunning)
            {
                if(spaceIsPressed)
                {
                isRunning = true; //starts timer
                }
            }
            
            if (spaceIsPressed && isRunning)
            {
                //timer
                timer += Time.deltaTime;
                UpdateScreen();

                //lose conditions
                if (timer >= gameDuration)
                {
                    isRunning = false;
                    EndGame();
                }
                if (lives == 0)
                {
                    isRunning = false;
                    EndGame();
                }
                if (hits == 2)
                {
                    isRunning = false;
                    EndGame();
                }
            }

        }
        

        // changes the isRunning boolean to false and changes instruction
        //text on UI to read win or lose
        
        private void EndGame()
        {
            isRunning = false;
            if (hits == 2)
            {
            winScene.gameObject.SetActive(true);
            }
            else
            {
            loseScene.gameObject.SetActive(true) ;
            }
        }

    public void EnableInputs()
    {
        //parry = playerInput.currentActionMap.FindAction("Parry");
        //block = playerInput.currentActionMap.FindAction("Block");
        attack = playerInput.currentActionMap.FindAction("Attack");
        restart = playerInput.currentActionMap.FindAction("Restart");
        quit = playerInput.currentActionMap.FindAction("Quit");

        restart.started += Restart_started;
        quit.started += Quit_started;
    }

    #region Input Actions
    private void Attack_started(InputAction.CallbackContext obj)
    {
        if(!spaceIsPressed)
        {
            startGameScreen.gameObject.SetActive(false);
            spaceIsPressed = true;
        }
        else if(spaceIsPressed)
        {

        }
    }
    private void Quit_started(InputAction.CallbackContext obj)
    {
        //Loads Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        //Reloads current scene
        SceneManager.LoadScene(4);
    }

    #endregion

    // CheckInput is called when the player presses a key during an active game.
    // It validates the key pressed against the allowed inputs and provides feedback
    // to the player. If the input is correct, the game continues, but if it's incorrect,
    // the player loses a life, and the game may end if there are no more lives.
    // It also updates the user interface to reflect the result.
    private void CheckInput() 
    {
            KeyCode keyPressed = KeyCode.None;

            if (Input.GetKeyDown(KeyCode.F))
            {
                keyPressed = KeyCode.F;
                Debug.Log("F was pressed");
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                keyPressed = KeyCode.A;
                Debug.Log("A");
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                keyPressed = KeyCode.Space;
                Debug.Log("Space");
            }

            IsValidInput(keyPressed);

        }

        //isValidInput checks if the key that the player has entered is the correct key 
        //corresponding to the action given.
        private void IsValidInput(KeyCode keyPressed)
        {
            if (keyPressed == action)
            {
                hits++;
                instructionText.text = "CORRECT!";

            }
            else
            {
                lives--;
                instructionText.text = "MISSED!";

            }

        }
        //returns True if correct is pressed, and false if the key entered is not correct

        // DisplayRandomInstruction is a coroutine that presents a random combat instruction to the player.
        // It sets the instruction text on the user interface, waits for a few seconds, and then updates
        // the text to indicate that the player missed. After a short delay, it prompts the player to get ready
        // for the next instruction.
        private void DisplayRandomInstruction()
        {
            if (roundCount == 0)
            {
                instructionText.text = "READY!";
                instructionText.text = "SET!";
                instructionText.text = "FIGHT!";
            }

            instructionText.text = GetRandomInstruction();
            Debug.Log("Entered Display Random");

        }
        ///<summary>
        ///Description: GetRandomInstruction holds a list of instructions and randomly choses an instruction to produce for the user
        ///<return>Returns key instructions the player must press 
        ///</return>
        ///</summary>
        private string GetRandomInstruction()
        {
            string[] instructions = { "Parry (Press F)", "Block (Press A)", "Attack (Press Space)" };
            int randomIndex = Random.Range(0, instructions.Length);
            Debug.Log(instructions[randomIndex]);
            if (randomIndex == 0)
            {
                action = KeyCode.F;
            }
            if (randomIndex == 1)
            {
                action = KeyCode.A;
            }
            if (randomIndex == 2)
            {
                action = KeyCode.Space;
            }
            Debug.Log("Get Display Random");
            return instructions[randomIndex];

        }


    }


