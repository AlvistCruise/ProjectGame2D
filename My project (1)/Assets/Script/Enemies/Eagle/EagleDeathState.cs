using UnityEngine;

public class EagleDeathState : EagleBaseState
{
    private float timeEntered;
    private const float timeToDespawn = 3f;

    public override void EnterState(Eagle eagle)
    {
        timeEntered = Time.time;

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
            eagleRb2D.linearVelocity = Vector2.zero;
            eagleRb2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public override void UpdateState(Eagle eagle)
    {
        if (Time.time >= timeEntered + timeToDespawn)
        {
            if (eagle.gameObject != null)
            {
                Object.Destroy(eagle.gameObject);
            }
        }
    }

    public override void ExitState(Eagle eagle)
    {
    }
}