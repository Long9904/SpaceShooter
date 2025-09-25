using System;
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
        if (collision.CompareTag("Obstacle"))
        {
            Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
