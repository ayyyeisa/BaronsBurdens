//PlayerDuel.cs
//Carl Crumer
//Creation Date: October 24 2023
//
// The script the player functions of the game,input, and User Interface
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDuel : MonoBehaviour
{
   //Text for instructions, the player's lives, and a time 
   private TMP_Text instructionText;
   private  TMP_Text livesText;
   private TMP_Text timerText;
    //player gets 2 lives, the game lasts for 20 seconds
    private int lives = 2;
    private float gameDuration = 20f;
    private float timer = 0f;
    private bool isRunning = false;

    //
    private KeyCode[] validInputs = { KeyCode.F, KeyCode.A, KeyCode.Space };


    // Start is called before the first frame update
    void Start()
    {
        StartGame();
        UpdateScreen();
    }
    //Updates the user interface,updates the player's lives and time remaining
    private void UpdateScreen()
    {
        livesText.text = "Lives: " + lives;
        timerText.text = "Time: " + (int)(gameDuration - timer);
    }
   
    //Start the game by initializing game variables, resetting the timer,
    // and displaying the initial instruction. This function is called
    // at the beginning and when the player restarts the game.
    private void StartGame()
    {
        isRunning = true;
        timer = 0f;
        lives = 2;
       Update();
       // Display the first instruction to the player
        StartCoroutine(DisplayRandomInstruction());
    }
    // The Update function is called once per frame and handles game state updates,
    // track the timer, check for player input, and ends the game
    // when the timer reaches the game duration. It also updates the user interface.
    void Update()
    {
        if (isRunning)
        {
            //timer
            timer += Time.deltaTime; 
            UpdateScreen();

            if (timer >= gameDuration)
            {
                EndGame();
            }
        }

        if (Input.anyKeyDown && isRunning)
        {
            //check for player input
            CheckInput();
        }
    }
   // changes the isRunning boolean to false and changes instruction
   //text on UI to read "Game Over"
private void EndGame()
{
    isRunning = false;
    instructionText.text = "Game Over";

}
    // CheckInput is called when the player presses a key during an active game.
    // It validates the key pressed against the allowed inputs and provides feedback
    // to the player. If the input is correct, the game continues, but if it's incorrect,
    // the player loses a life, and the game may end if there are no more lives.
    // It also updates the user interface to reflect the result.
    private void CheckInput(){
    KeyCode keyPressed = KeyCode.None;

    if (Input.GetKeyDown(KeyCode.F))
    {
        keyPressed = KeyCode.F;
    }
    else if (Input.GetKeyDown(KeyCode.A))
    {
        keyPressed = KeyCode.A;
    }
    else if (Input.GetKeyDown(KeyCode.Space))
    {
        keyPressed = KeyCode.Space;
    }

    if (keyPressed != KeyCode.None)
    {
        if (IsValidInput(keyPressed))
        {
            instructionText.text = "Correct!";
            StartCoroutine(DisplayRandomInstruction());
        }
        else
        {
            lives--;
            if (lives <= 0)
            {
                EndGame();
            }
            else
            {
                instructionText.text = "Incorrect! Try Again.";
            }
        }
        UpdateScreen();
    }
}
    //isValidInput checks if the key that the player has entered is the correct key 
    //corresponding to the instruction given.
private bool IsValidInput(keyCode keyPressed)
{
    foreach (KeyCode validKey in validInputs)
    {
        if (keyPressed == validKey)
        {
            return true;
        }
    }
    return false;

}
    //returns True if correct is pressed, and false if the key entered is not correct

    // DisplayRandomInstruction is a coroutine that presents a random combat instruction to the player.
    // It sets the instruction text on the user interface, waits for a few seconds, and then updates
    // the text to indicate that the player missed. After a short delay, it prompts the player to get ready
    // for the next instruction.
    private IEnumerator DisplayRandomInstruction()
{

    instructionText.text = GetRandomInstruction();
    yield return WaitForSeconds(3f);
    instructionText.text = "Missed!";
    yield return new WaitForSeconds(1f);
    instructionText.text = "Get Ready...";
    yield return new WaitForSeconds(1f);
    instructionText.text = "Fight!";
}
//GetRandomInstruction holds a list of instructions and randomly choses 
//an instruction to produce for the user
private string GetRandomInstruction()
{
    string[] instructions = { "Parry (Press F)", "Block (Press A)", "Attack (Press Space)" };
    int randomIndex = Random.Range(0, instructions.Length);
    return instructions[randomIndex];
}


}
