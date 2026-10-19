using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    public float lifetime = 3f;
    private bool hasHit = false;

    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.usedByEffector = false;
        }
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Player"))
        {
            hasHit = true;
            PlayerControllerMain player = other.GetComponent<PlayerControllerMain>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            hasHit = true;
            PlayerControllerMain player = collision.gameObject.GetComponent<PlayerControllerMain>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}