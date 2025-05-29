// EagleAttackingState.cs (2D Version)
using UnityEngine;

public class EagleAttackingState : EagleBaseState
{
    private float _lastAttackTime;

    public override void EnterState(Eagle eagle)
    {
        Debug.Log("Eagle entering Attacking State (2D).");
        _lastAttackTime = Time.time - eagle.attackCooldown; // Allow immediate shot
    }

    public override void UpdateState(Eagle eagle)
    {
        if (eagle.player == null)
        {
            eagle.TransitionToState(eagle.IdleState);
            return;
        }

        float distanceToPlayer = eagle.DistanceToPlayer();

        Vector2 currentEaglePos2D = eagle.transform.position;
        Vector2 playerPos2D = eagle.player.position;
        Vector2 directionToPlayer = (playerPos2D - currentEaglePos2D).normalized;
        eagle.FaceDirection2D(directionToPlayer); // Always face player in attack state

        if (distanceToPlayer > eagle.attackRange)
        {
            if (distanceToPlayer <= eagle.detectionRange)
            {
                eagle.TransitionToState(eagle.ChasingState);
            }
            else
            {
                eagle.TransitionToState(eagle.IdleState);
            }
            return;
        }

        if (Time.time >= _lastAttackTime + eagle.attackCooldown)
        {
            eagle.Shoot(); // Eagle.Shoot() is now 2D ready
            _lastAttackTime = Time.time;
        }
    }

    public override void ExitState(Eagle eagle)
    {
        Debug.Log("Eagle exiting Attacking State (2D).");
    }
}