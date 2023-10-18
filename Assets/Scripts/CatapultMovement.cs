using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatapultMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D Rb2D;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject enemyKnight;
    [SerializeField] private GameObject enemyKnightSpawn;
    [SerializeField] private GameObject catapultAmmo;
    [SerializeField] private GameObject catapultAmmoSpawn;

    private InputAction move;
    private InputAction shoot;

    public Coroutine EnemyKnightRef;

    private bool isMoving;
    private bool didShoot;
    private float moveDirection;
    public static bool IsAmmoDestroyed;
    private int numOfEnemyKnights = 20;

    // Start is called before the first frame update
    void Start()
    {
        EnableInputs();
        isMoving = false;
        didShoot = false;
        IsAmmoDestroyed = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            moveDirection = move.ReadValue<float>();
        }

        if (numOfEnemyKnights > 0)
        {
            if (EnemyKnightRef == null)
            {
                EnemyKnightRef = StartCoroutine(EnemyKnightTimer());
            }
        }

    }

    public IEnumerator EnemyKnightTimer()
    {
        SpawnEnemyKnight();
        yield return new WaitForSeconds(2f);
        EnemyKnightRef = null;
        numOfEnemyKnights--;

    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection * moveSpeed, 0);
        }

        if (!isMoving)
        {
            Rb2D.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void EnableInputs()
    {
        playerInput.currentActionMap.Enable();

        move = playerInput.currentActionMap.FindAction("Move");
        shoot = playerInput.currentActionMap.FindAction("Shoot");

        move.started += Move_started;
        move.canceled += Move_canceled;
        shoot.started += Shoot_started;
    }

    public void SpawnEnemyKnight()
    {
        Vector2 playerPause = enemyKnightSpawn.transform.position;
        GameObject temp = Instantiate(enemyKnight, playerPause, Quaternion.identity);

        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
    }

    public void SpawnAmmo()
    {
        if (didShoot)
        {
            Vector2 playerPause = catapultAmmoSpawn.transform.position;
            GameObject temp = Instantiate(catapultAmmo, playerPause, Quaternion.identity);

            temp.GetComponent<Rigidbody2D>().velocity = new Vector2(-7, 0);

            didShoot = false;
            IsAmmoDestroyed = false;
        }
    }

    private void Shoot_started(InputAction.CallbackContext obj)
    {
        if (IsAmmoDestroyed)
        {
            print("Shooting with catapult");
            didShoot = true;

            SpawnAmmo();
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

    public void OnDestroy()
    {
        move.started -= Move_started;
        move.canceled -= Move_canceled;
        shoot.started -= Shoot_started;
    }
}
