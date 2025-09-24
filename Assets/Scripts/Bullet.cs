using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    //[SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float bound = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.right * speed;
    }

    private void Update()
    {
        if (transform.position.x >= bound)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstracle"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
