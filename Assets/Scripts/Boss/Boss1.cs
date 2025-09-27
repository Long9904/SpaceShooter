using System.Collections;
using UnityEngine;

public class Boss1 : MonoBehaviour
{


    [SerializeField] private float bossSpeed = 1f;
    public Transform player;

    [Header("Movement Limits")]
    public float minX = -1f, maxX = 6f;    // X boundaries
    public float minY = -3f, maxY = 3f;    // Y boundaries

    private Animator animator;

    private float punchCooldown;
    private float timerPunch;
    private float chargeSpeed = 8f;

    [Header("Fire Points")]
    public GameObject laserPrefab;
    public Transform firePoint;
    public float bulletSpeed = 7f;
    public float fireRate = 3f;
    private float fireTimer;

    [Header("Health")]
    public float maxHealth;
    private float health;

    [Header("Destroy Effect")]
    [SerializeField] private GameObject destroyEffect;

    [Header("Boss Clone")]
    private bool isClone = false;
    private bool hasSplit = false;
    [SerializeField] private GameObject bossPrefab; // prefab boss
    [SerializeField] private int cloneCount = 2;    // 
    [SerializeField] private float spawnRadius = 5f; //

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Boss disappears at start
        StartCoroutine(MoveIn());

        punchCooldown = Random.Range(5f, 10f);
        timerPunch = 0f;
        health = maxHealth;
        BossUIController.Instance.UpdateBossHealthSlider(health, maxHealth);

        if (isClone)
        {
            // Rotaion z
            transform.Rotate(0f, 0f, -90);
            maxHealth = 5;
            health = maxHealth;
        }
        AudioManagement.instance.PlayBackgroundMusic();
    }

    void Update()
    {

        FlowPlayer();
        timerPunch += Time.deltaTime;
        fireTimer += Time.deltaTime;
        if (timerPunch >= punchCooldown)
        {
            StartCoroutine(BossPunch());
            timerPunch = 0f;
            punchCooldown = Random.Range(5f, 10f); // reset timer with new random cooldown
        }
        else if (fireTimer >= fireRate && !animator.GetBool("isCharging"))
        {
            StartCoroutine(DelayShoot());
            fireTimer = 0f;
            fireRate -= 0.1f; // Increase fire rate over time
            if (fireRate <= 0.4f) fireRate = 0.5f;

        }

    }

    IEnumerator MoveIn()
    {
        Vector3 spawnPosition = new Vector3(7f, 0f, 0f);
        while (Vector3.Distance(transform.position, spawnPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnPosition, 2f * Time.deltaTime);
            yield return null;
        }
    }

    // Boss movement logic (flying around the screen with boundaries)

    private void FlowPlayer()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);

        // Handle Y  boundaries
        if (player.position.y > maxY)
        {
            targetPosition.y = maxY;
        }
        else if (player.position.y < minY)
        {
            targetPosition.y = minY;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, bossSpeed * Time.deltaTime);
    }

    private IEnumerator BossPunch()
    {
        // 1. Set animator isCharging to true
        // 2. Stop all boss actions like shooting and moving
        // 3. Move boss quickly towards the player
        // 4. Boss returns to original position
        // 5. Set animator isCharging to false

        animator.SetBool("isCharging", true);

        Vector3 originalPosition = transform.position;
        Vector3 playerPos = player.position;

        Vector3 direction = (playerPos - originalPosition).normalized;

        Vector3 targetPosition = playerPos - direction;


        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, chargeSpeed * Time.deltaTime);
            yield return null;
        }

        animator.SetBool("isCharging", false);
    }

    void ShootLaser()
    {

        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);


        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector3 playerPos = (player.position - firePoint.position).normalized;
            rb.linearVelocity = playerPos * bulletSpeed;
        }
    }

    IEnumerator DelayShoot()
    {
        yield return new WaitForSeconds(0.2f);
        ShootLaser();

  
        if (health <= maxHealth * 0.75f)
        {
            yield return new WaitForSeconds(0.3f); // delay 
            ShootLaser();
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }


    private void TakeDamage(int damage)
    {
        health -= damage;
        BossUIController.Instance.UpdateBossHealthSlider(health, maxHealth);

        if (health <= maxHealth / 2 && !hasSplit)
        {
            SplitBoss();
            hasSplit = true;
        }

        AudioManagement.instance.PlayHitEn();

        if (health <= 0)
        {
            gameObject.SetActive(false);
            // Clone the destroy effect at the player's position and rotation
            Instantiate(destroyEffect, transform.position, transform.rotation);
            return;
        }

    }

    public void SplitBoss()
    {
        for (int i = 0; i < cloneCount; i++)
        {
            Vector3 spawnPos = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;

            GameObject clone = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

            clone.GetComponent<Boss1>().isClone = true;
        }
    }
}