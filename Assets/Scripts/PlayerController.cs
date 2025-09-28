using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;


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

    [Header("Gas Bar")]
    [SerializeField] private float gas;
    [SerializeField] private float gasRegen;
    [SerializeField] private float gasDownRate;
    [SerializeField] private float maxGas;

    [Header("Destroy Effect")]
    [SerializeField] private GameObject destroyEffect;

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

    [Header("Shield")]
    [SerializeField] private GameObject shieldPrefab;


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
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);

        health = maxHealth;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);

        gas = maxGas;
        UIController.Instance.UpdateGasSlider(gas, maxGas);
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
                AudioManagement.instance.PlayShooter();
            }
            Parallax.instance.globalSpeed = boost;
            GasDown();
            if (gas <= 0)
            {
                OnGasEmpty();
            }

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
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
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

    public void ExitBoost()
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
        else if (collision.CompareTag("Gas"))
        {
            FillGas(gasRegen);
            Destroy(collision.gameObject);
            AudioManagement.instance.PlayLootItem();
        }
        else if (collision.CompareTag("Shield"))
        {
            StartCoroutine(ActiveShield());
            Destroy(collision.gameObject);
            AudioManagement.instance.PlayLootItem();

        }
        else if (collision.gameObject.CompareTag("BulletEn"))
        {
            TakeDamage(DamageUpdateByTime());
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(DamageUpdateByTime());
            Destroy(collision.gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void TakeDamage(int damage)
    {
        if(shieldPrefab.activeSelf) return;
        health -= damage;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        if (health <= 0)
        {
            boost = 1f;
            
            // Clone the destroy effect at the player's position and rotation
            Instantiate(destroyEffect, transform.position, transform.rotation);
            UIEndGame.Instance.ShowGameOver();
            gameObject.SetActive(false);
            AudioManagement.instance.PlayHit();
            Time.timeScale = 0f;
            return;
        }
        StartCoroutine(GetHit());
        AudioManagement.instance.PlayHit();

    }
    private void GasDown()
    {
        gas -= gasDownRate * Time.deltaTime;
        UIController.Instance.UpdateGasSlider(gas, maxGas);
    }
    private void FillGas(float amount)
    {
        gas += amount;
        if (gas > maxGas) gas = maxGas;
        UIController.Instance.UpdateGasSlider(gas, maxGas);
    }

    private IEnumerator ActiveShield()
    {
        shieldPrefab.SetActive(true);
        yield return new WaitForSeconds(5f);
        shieldPrefab.SetActive(false);
    }
    private IEnumerator GetHit()
    {
        animator.SetBool("isHit", true);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isHit", false);
    }

    private int DamageUpdateByTime()
    {
        float time = Time.timeSinceLevelLoad;
        // Increase damage by 1 every 10 seconds, up to a maximum of 3
        int damage = Mathf.Min(1 + Mathf.FloorToInt(time / 10f), 3);
        return damage;
    }
    private void OnGasEmpty()
    {
        TakeDamage(3);
        gas = maxGas / 2f;
        UIController.Instance.UpdateGasSlider(gas, maxGas);
    }
}
