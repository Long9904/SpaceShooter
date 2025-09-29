
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    //[SerializeField] private int damage = 1;
    [SerializeField] private float bound = 10f;
    [SerializeField] private GameObject explosionEffectPrefab;
    private enum targetType { Player, Enemy };
    [SerializeField] private targetType target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }

    private void Update()
    {
        if (transform.position.x >= bound || transform.position.x <= -bound)
        {
            Destroy(gameObject);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {

       
    }
}
