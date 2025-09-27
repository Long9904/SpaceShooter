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
    public GameObject superBulletPrefab;
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
    [SerializeField] private int cloneCount = 3;    // 


    [Header("Bullet Hell")]
    public float bulletHellCooldown = 2f;
    private float bulletHellTimer;
    public int spiralBullets = 30;
    public float spiralDelay = 0.05f;
    public float spiralAngleStep = 10f;
    private float angle = 0f;


    private Vector3 randomTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(MoveIn());

        punchCooldown = Random.Range(5f, 10f);
        timerPunch = 0f;

        if (isClone)
        {
            transform.Rotate(0f, 0f, -90);
            maxHealth = 8;
            health = maxHealth;

            randomTarget = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                transform.position.z
            );
        }
        else
        {
            health = maxHealth;
            BossUIController.Instance.UpdateBossHealthSlider(health, maxHealth);
        }

        AudioManagement.instance.PlayBackgroundMusic();
    }


    void Update()
    {

        FlowPlayer();

        timerPunch += Time.deltaTime;

        fireTimer += Time.deltaTime;

        bulletHellTimer += Time.deltaTime;

        if (timerPunch >= punchCooldown)
        {
            StartCoroutine(BossPunch());
            timerPunch = 0f;
            punchCooldown = Random.Range(5f, 10f); // reset timer with new random cooldown
        }
        if (fireTimer >= fireRate && !animator.GetBool("isCharging") && health >= maxHealth * 0.6f)
        {
            StartCoroutine(DelayShoot());
            fireTimer = 0f;
            fireRate -= 0.1f; // Increase fire rate over time
            if (fireRate <= 0.4f) fireRate = 0.5f;
        }
        if (!isClone 
            && bulletHellTimer >= bulletHellCooldown
            && !animator.GetBool("isCharging") 
            && health < maxHealth * 0.6f
            && health > maxHealth * 0.3)
        {
            StartCoroutine(SpiralBulletHell());
            bulletHellTimer = 0f;
        }
        if (fireTimer >= fireRate 
            && !isClone 
            && !animator.GetBool("isCharging") 
            && health <= maxHealth * 0.3f)
        {
            StartCoroutine(DelayShoot());
            fireTimer = 0f;
            fireRate -= 0.1f; 
            if (fireRate <= 0.4f) fireRate = 1.1f;
        }

    }

    IEnumerator MoveIn()
    {
        if (isClone) yield break; // Clones do not move in
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

        if (isClone)
        {

            transform.position = Vector3.MoveTowards(transform.position, randomTarget, (bossSpeed / 2) * Time.deltaTime);

            if (Vector3.Distance(transform.position, randomTarget) < 0.1f)
            {
                randomTarget = new Vector3(
                    Random.Range(minX, maxX),
                    Random.Range(minY, maxY),
                    transform.position.z
                );
            }
            return;
        }

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

        if (isClone) yield break; // Clones do not punch

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

    void ShootBullet()
    {
        if ( health <= maxHealth * 0.3f && !isClone)
        {
            // Shoot super bullet
            GameObject superBullet = Instantiate(superBulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbSuper = superBullet.GetComponent<Rigidbody2D>();
            if (rbSuper != null)
            {
                Vector3 playerPos = (player.position - firePoint.position).normalized;
                rbSuper.linearVelocity = playerPos * (bulletSpeed + 2f); // Super bullet is faster
            }
            return;
        }

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
        ShootBullet();


        if (health <= maxHealth * 0.75f)
        {
            yield return new WaitForSeconds(0.3f); // delay 
            ShootBullet();
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

        if (!isClone)
        {
            BossUIController.Instance.UpdateBossHealthSlider(health, maxHealth);
        }

        if (!isClone && health <= maxHealth * 0.6f && !hasSplit)
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
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            Vector3 spawnPos = new Vector3(randomX, randomY, transform.position.z);

            GameObject clone = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

            clone.GetComponent<Boss1>().isClone = true;
        }
    }

    private IEnumerator SpiralBulletHell()
    {
       
        for (int i = 0; i < spiralBullets; i++)
        {

            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0f);

            GameObject bullet = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = bulletMoveVector * bulletSpeed;

            angle += spiralAngleStep;

            yield return new WaitForSeconds(spiralDelay);
        }
    }

}