using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;


    private Rigidbody2D rb;
    private Vector2 playerDirection;
    private Animator animator;

    [SerializeField] private float moveSpeed;

    /* 
     - Boost variables will not effect the player's actual speed, but will be used in other scripts to modify behavior like shooting rate, scense, background, enemy speed, obstacle speed.

     - Space ship will move at normal speed, but the game will feel faster because this will make player feel their ship are boosting.

     - Boost will be activated when player holds down space bar, and deactivated when released.
     - How to use : PlayerController.Instance.boost
    */

    public float boost = 1f;

    [SerializeField] private float boostPower = 5f;
    private bool isBoosting = false;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Get the Rigidbody component attached to the player GameObject
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Time.timeScale > 0)
        {
            // Go to Edit -> Project Settings -> Input Manager to see the input axes
            float directionX = Input.GetAxisRaw("Horizontal");
            float directionY = Input.GetAxisRaw("Vertical");

            // Check in the animator for the parameters "moveX" and "moveY"

            animator.SetFloat("moveX", directionX);
            animator.SetFloat("moveY", directionY);


            // Create a new Vector2 based on the input axes
            playerDirection = new Vector2(directionX, directionY).normalized;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EnterBoost();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                ExitBoost();
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
            Parallax.instance.globalSpeed = boost;
        }
    }


    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed, playerDirection.y * moveSpeed);
    }

    private void EnterBoost()
    {
        animator.SetBool("isBoost", true);
        boost = boostPower;
        isBoosting = true;

    }

    private void ExitBoost()
    {
        animator.SetBool("isBoost", false);
        boost = 1f;
        isBoosting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Gas"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Shield"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("BulletEn"))
        {
            Destroy(collision.gameObject);
        }
    }
}
