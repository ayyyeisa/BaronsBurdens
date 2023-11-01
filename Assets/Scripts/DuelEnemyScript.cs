using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;

public class DuelEnemyScript : MonoBehaviour
{
    public GameObject Block;
    public GameObject Attack;
    public GameObject Parry;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    int playerHP = 2;
    int enemyHP = 2;

    //Text for instructions, the player's lives, and a time
    //use this 
    public TMP_Text livesText;
    public TMP_Text timerText;
    public TMP_Text enemyLivesText;
    private float gameDuration = 30f;
    private float timer = 0f;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        Block.SetActive(false);
        Parry.SetActive(false);
        Attack.SetActive(false);
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
        InputSystem.DisableDevice(Keyboard.current);

        StartGame();
        UpdateScreen();

        //Start the coroutine defined below named EnemyCoroutine.
        StartCoroutine(EnemyCoroutine());
    }

    //Updates the user interface,updates the player's lives and time remaining
    private void UpdateScreen()
    {
        livesText.text = "Lives: " + playerHP;
        timerText.text = "Time: " + (int)(gameDuration - timer);
        enemyLivesText.text = "Enemy Lives: " + enemyHP;
    }

    //Start the game by initializing game variables, resetting the timer,
    // and displaying the initial instruction. This function is called
    // at the beginning and when the player restarts the game.
    private void StartGame()
    {

        //use this 
        isRunning = true;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //use some of this 
        if (isRunning)
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
            if (playerHP == 0)
            {
                isRunning = false;
                EndGame();
            }
            if (enemyHP == 0)
            {
                isRunning = false;
                EndGame();
            }
        }
    }

    IEnumerator EnemyCoroutine()
    {
        int rand = Random.Range(1, 6);


        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(rand);
        InputSystem.EnableDevice(Keyboard.current);
        int rand2 = Random.Range(1, 4);
        if (rand2 == 1)
        {
            StabAttack();
        }
        else if (rand2 == 2)
        {
            HeavyAttack();
        }
        else if (rand2 == 3)
        {
            GuardDown();
        }

        

        

        yield return new WaitForSeconds(2);
        if (rand2 == 1)
        {
            if (DidBlock() == true)
            {
                StabAttack();
            }
            else
            {
                playerHP--;
                StabAttack();
            }
        }
        else if (rand2 == 2)
        {
            if (DidParry() == true)
            {
                HeavyAttack();
            }
            else
            {
                playerHP--;
                HeavyAttack();
            }
        }
        else if (rand2 == 3)
        {
            if (DidAttack() == true)
            {
                enemyHP--;
                GuardDown();
            }
            else
            {
                GuardDown();
            }
        }

        InputSystem.DisableDevice(Keyboard.current);

    }

    private void StabAttack()
    {
        if (Block.activeSelf == true)
        {
            Block.SetActive(false);
        }
        else
        {
            Block.SetActive(true);
        }
    }

    private void HeavyAttack()
    {
        if (Parry.activeSelf == true)
        {
            Parry.SetActive(false);
        }
        else
        {
            Parry.SetActive(true);
        }
    }

    private void GuardDown()
    {
        if (Attack.activeSelf == true)
        {
            Attack.SetActive(false);
        }
        else
        {
            Attack.SetActive(true);
        }
    }

    private bool DidBlock()
    {
        bool pressedA = false;
        if (Input.GetKeyDown(KeyCode.A))
        {
            pressedA = true;
        }

        return pressedA;
    }

    private bool DidParry()
    {
        bool pressedF = false;
        if (Input.GetKeyDown(KeyCode.F))
        {
            pressedF = true;
        }

        return pressedF;
    }

    private bool DidAttack()
    {
        bool pressedBar = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressedBar = true;
        }

        return pressedBar;
    }

    // changes the isRunning boolean to false and changes instruction
    //text on UI to read win or lose, and goes back to main menu

    private void EndGame()
    {
        isRunning = false;
        if (enemyHP == 0)
        {
            WinScreen.SetActive(true);
        }
        else
        {
            LoseScreen.SetActive(true);
        }
        EditorSceneManager.LoadScene("MainMenu");
    }
}
