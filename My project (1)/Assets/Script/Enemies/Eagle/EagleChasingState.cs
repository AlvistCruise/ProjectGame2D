using UnityEngine;

public class EagleChasingState : EagleBaseState
{
    public override void EnterState(Eagle eagle)
    {
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

        Vector3 newPosition = eagle.transform.position + (Vector3)directionToPlayer * eagle.moveSpeed * Time.deltaTime;
        newPosition.z = eagle.transform.position.z;
        eagle.transform.position = newPosition;

        eagle.FaceDirection2D(directionToPlayer);
    }

    public override void ExitState(Eagle eagle)
    {
    }
}