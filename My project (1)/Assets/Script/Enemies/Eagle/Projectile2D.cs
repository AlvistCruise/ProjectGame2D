// Projectile2D.cs
using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    // public float speed = 10f; // Speed can also be set by the Eagle
    public float lifetime = 3f;
    // public Rigidbody2D rb; // Can be assigned if not moving via transform

    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        // If not using Rigidbody velocity from Eagle:
        // rb.velocity = transform.right * speed; // Assuming sprite faces right
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Example: Damage Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Projectile hit Player!");
            // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            // if (playerHealth != null) playerHealth.TakeDamage(10);
            Destroy(gameObject); // Destroy projectile on hit
        }
        // Optionally destroy on hitting other things like walls
        // else if (other.CompareTag("Wall")) Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
         // Example: Damage Player if not using trigger
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Projectile hit Player!");
            // ... damage logic ...
            Destroy(gameObject);
        }
    }
}