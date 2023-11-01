using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DuelEnemyScript : MonoBehaviour
{
   [SerializeField] private GameObject Block;
    [SerializeField] private GameObject Attack;
    [SerializeField] private GameObject Parry;

    public Coroutine DuelEnemyRef;
    
    // Start is called before the first frame update
    void Start()
    {
        Block.SetActive(false);
        Parry.SetActive(false);
        Attack.SetActive(false);
        InputSystem.DisableDevice(Keyboard.current);

        //Start the coroutine defined below named EnemyCoroutine.
        if(DuelEnemyRef == null)
        {
            DuelEnemyRef = StartCoroutine(EnemyCoroutine());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemyCoroutine()
    {
        int playerHP = 2;
        int enemyHP = 2;
        
        int rand = Random.Range(1, 6);

   
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(rand);

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

        InputSystem.DisableDevice(Keyboard.current);

        if (rand2 == 1)
        {
            if (DidBlock() == true && Block.activeSelf == true)
            {

            }
            else
            {
                playerHP -= 1;
            }
        }
        else if (rand2 == 2)
        {
            if (DidParry() == true && Parry.activeSelf == true)
            {

            }
            else
            {
                playerHP -= 1;
            }
        }
        else if (rand2 == 3)
        {
            if (DidAttack() == true && Attack.activeSelf == true)
            {
                enemyHP -= 1;
            }
            else
            {

            }
        }

        yield return new WaitForSeconds(2);
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
}
