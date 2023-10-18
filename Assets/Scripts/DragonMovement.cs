using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragonMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D Rb2D;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject pF;
    [SerializeField] private GameObject enemyFireballSpawn;
    [SerializeField] private GameObject eF;


    public Coroutine EnemyFireballRef;
    public Coroutine GameTimerRef;
    private bool isGameRunning;

    public static bool isFireballDestroyed;

    private InputAction move;
    private InputAction fireball;

    private bool isMoving;
    public bool didFire;
    private float moveDirection;
    private int numOfLives = 2;

    // Start is called before the first frame update
    void Start()
    {
        EnableInputs();

        isMoving = false;
        didFire = false;
        isFireballDestroyed = true;
        isGameRunning = false;

        if (GameTimerRef == null)
        {
            GameTimerRef = StartCoroutine(GameTimer());
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            moveDirection = move.ReadValue<float>();
        }

        if (isGameRunning)
        {
            if (EnemyFireballRef == null)
            {
                EnemyFireballRef = StartCoroutine(EnemyFireballTimer());
            }
        }


    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveDirection * moveSpeed);
        }

        if (!isMoving)
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.name == "EnemyFireball(Clone)") && (numOfLives == 0))
        {
            Destroy(gameObject);
        }
        else if ((collision.transform.name == "EnemyFireball(Clone)") && (numOfLives > 0))
        {
            numOfLives--;
        }
    }



    public IEnumerator EnemyFireballTimer()
    {
        SpawnEnemyFireball();
        yield return new WaitForSeconds(1f);
        EnemyFireballRef = null;

    }

    public IEnumerator GameTimer()
    {
        isGameRunning = true;
        yield return new WaitForSeconds(20f);
        GameTimerRef = null;
        isGameRunning = false;
    }

    public void EnableInputs()
    {
        playerInput.currentActionMap.Enable();

        move = playerInput.currentActionMap.FindAction("Move");
        fireball = playerInput.currentActionMap.FindAction("Fireball");

        move.started += Move_started;
        move.canceled += Move_canceled;
        fireball.started += Fireball_started;
    }

    private void Fireball_started(InputAction.CallbackContext obj)
    {

        if (isFireballDestroyed)
        {
            didFire = true;
            SpawnFireball();

            print("Fireball shot");

            isFireballDestroyed = false;

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

    public void SpawnFireball()
    {
        if (didFire)
        {
            Vector2 playerPause = gameObject.transform.position;
            playerPause.x += 0.5f;
            GameObject temp = Instantiate(pF, playerPause, Quaternion.identity);

            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(7, 0);
        }
    }

    public void SpawnEnemyFireball()
    {
        Vector2 playerPause = enemyFireballSpawn.transform.position;
        GameObject temp = Instantiate(eF, playerPause, Quaternion.identity);

        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(-7, 0);
    }

    public void OnDestroy()
    {
        move.started -= Move_started;
        move.canceled -= Move_canceled;
        fireball.started -= Fireball_started;
    }
}
