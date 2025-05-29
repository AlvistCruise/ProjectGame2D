// EagleDeathState.cs (2D Version)
using UnityEngine;

public class EagleDeathState : EagleBaseState
{
    private float _timeEntered;
    private const float TIME_TO_DESPAWN = 3f;

    public override void EnterState(Eagle eagle)
    {
        Debug.Log("Eagle entering Death State (2D).");
        _timeEntered = Time.time;

        if (eagle.animator != null)
        {
            eagle.animator.SetTrigger("Die");
        }

        Collider2D eagleCollider = eagle.GetCollider2D();
        if (eagleCollider != null)
        {
            eagleCollider.enabled = false;
        }

        Rigidbody2D eagleRb2D = eagle.GetRigidbody2D();
        if (eagleRb2D != null)
        {
            eagleRb2D.velocity = Vector2.zero; // Stop any movement
            eagleRb2D.isKinematic = true; // Or eagleRb2D.simulated = false; to remove from physics simulation
            // Optionally, if you want it to fall with gravity and not be kinematic:
            // eagleRb2D.gravityScale = 1f;
        }
    }

    public override void UpdateState(Eagle eagle)
    {
        if (Time.time >= _timeEntered + TIME_TO_DESPAWN)
        {
            if (eagle.gameObject != null)
            {
                Object.Destroy(eagle.gameObject);
            }
        }
    }

    public override void ExitState(Eagle eagle)
    {
        // This state typically doesn't exit
    }
}