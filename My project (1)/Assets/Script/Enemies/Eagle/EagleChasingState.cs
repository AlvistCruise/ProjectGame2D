// EagleChasingState.cs (2D Version)
using UnityEngine;

public class EagleChasingState : EagleBaseState
{
    public override void EnterState(Eagle eagle)
    {
        Debug.Log("Eagle entering Chasing State (2D).");
    }

    public override void UpdateState(Eagle eagle)
    {
        if (eagle.player == null)
        {
            eagle.TransitionToState(eagle.IdleState);
            return;
        }

        float distanceToPlayer = eagle.DistanceToPlayer();

        if (distanceToPlayer < eagle.attackRange)
        {
            eagle.TransitionToState(eagle.AttackingState);
            return;
        }
        if (distanceToPlayer > eagle.detectionRange)
        {
            eagle.TransitionToState(eagle.IdleState);
            return;
        }

        Vector2 currentEaglePos2D = eagle.transform.position;
        Vector2 playerPos2D = eagle.player.position;
        Vector2 directionToPlayer = (playerPos2D - currentEaglePos2D).normalized;

        // Move in 2D
        Vector3 newPosition = eagle.transform.position + (Vector3)directionToPlayer * eagle.moveSpeed * Time.deltaTime;
        newPosition.z = eagle.transform.position.z; // Ensure Z is maintained
        eagle.transform.position = newPosition;

        eagle.FaceDirection2D(directionToPlayer);
    }

    public override void ExitState(Eagle eagle)
    {
        Debug.Log("Eagle exiting Chasing State (2D).");
    }
}