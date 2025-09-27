using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossPlayerController : MonoBehaviour
{
    public static BossPlayerController Instance;


    private Rigidbody2D rb;
    private Vector2 playerDirection;
    private Animator animator;

    [SerializeField] private float moveSpeed;

    [Header("Mana Bar")]
    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyRegen;

    [Header("Health Bar")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;


    [Header("Destroy Effect")]
    [SerializeField] private GameObject destroyEffect;

    /* 
     - Boost variables will not effect the player's actual speed, but will be used in other scripts to modify behavior like shooting rate, scense, background, enemy speed, obstacle speed.

     - Space ship will move at normal speed, but the game will feel faster because this will make player feel their ship are boosting.

     - Boost will be activated when player holds down space bar, and deactivated when released.
     - How to use : PlayerController.Instance.boost
    */

    [Header("Boost")]
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
        rb.freezeRotation = true;

        energy = maxEnergy;
        BossUIController.Instance.UpdateEnergySlider(energy, maxEnergy);

        health = maxHealth;
        BossUIController.Instance.UpdateHealthSlider(health, maxHealth);
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

        if (isBoosting)
        {
            // Consume mana 
            if (energy >= 0.2f) energy -= 0.2f;
            else ExitBoost();
        }
        else
        {
            // Recover mana
            if (energy < maxEnergy) energy += energyRegen;
        }
        BossUIController.Instance.UpdateEnergySlider(energy, maxEnergy);
    }

    private void EnterBoost()
    {
        if (energy > 10)
        {
            animator.SetBool("isBoost", true);
            boost = boostPower;
            isBoosting = true;
        }
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
            TakeDamage(1);
            Destroy(collision.gameObject);
        }     
        else if (collision.gameObject.CompareTag("BulletEn"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        } else if (collision.gameObject.CompareTag("Boss"))
        {
            TakeDamage(2);
       
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        BossUIController.Instance.UpdateHealthSlider(health, maxHealth);
        if (health <= 0)
        {
            boost = 1f;
            gameObject.SetActive(false);
            // Clone the destroy effect at the player's position and rotation
            Instantiate(destroyEffect, transform.position, transform.rotation);
            return;
        }
        StartCoroutine(GetHit());
    }

    private IEnumerator GetHit()
    {
        animator.SetBool("isHit", true);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isHit", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            TakeDamage(2);
        }
    }
}

