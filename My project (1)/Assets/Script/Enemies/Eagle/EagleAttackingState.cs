using UnityEngine;

public class EagleAttackingState : EagleBaseState
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

        Vector2 currentEaglePos2D = eagle.transform.position;
        Vector2 playerPos2D = eagle.player.position;
        Vector2 directionToPlayer = (playerPos2D - currentEaglePos2D).normalized;
        eagle.FaceDirection2D(directionToPlayer);

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

        if (eagle.CanShoot())
        {
            eagle.Shoot();
        }
    }

    public override void ExitState(Eagle eagle)
    {
    }
}