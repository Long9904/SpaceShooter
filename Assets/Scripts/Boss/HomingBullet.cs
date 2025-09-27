using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public Transform targetObject;
    public float speed = 8f;

    private Rigidbody2D rb;
    //[SerializeField] private int damage = 1;
    [SerializeField] private GameObject explosionEffectPrefab;
    private enum targetType { Player, Enemy };
    [SerializeField] private targetType target;
    [SerializeField] private float lifeTime;

    [Header("Homeing delay")]
    public float homingDelay;
    private bool isHoming = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (target == targetType.Player)
        {
            rb.linearVelocity = Vector2.right * speed;
        }
        else if (target == targetType.Enemy)
        {
            rb.linearVelocity = Vector2.left * speed;
        }

        Invoke(nameof(EnableHoming), homingDelay);

        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (!isHoming || targetObject == null) return;

        Vector2 direction = ((Vector2)targetObject.position - rb.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void Update()
    {
        if (transform.position.x >= 11f || transform.position.x <= -11f)
        {
            Destroy(gameObject);
        }
    }
    void EnableHoming()
    {
        isHoming = true;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") && target == targetType.Player)
        {
            Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);
            UIController.Instance.UpdateScore(10);
            Destroy(gameObject);
            Destroy(collision.gameObject);
            AudioManagement.instance.PlayHitEn();
        }
        else if (collision.CompareTag("Enemy") && target == targetType.Player)
        {
            Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);
            UIController.Instance.UpdateScore(15);
            Destroy(gameObject);
            Destroy(collision.gameObject);
            AudioManagement.instance.PlayHitEn();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Boss"))
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
