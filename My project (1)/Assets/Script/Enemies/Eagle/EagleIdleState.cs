using UnityEngine;

public class EagleIdleState : EagleBaseState
{
    private bool movingRight = true;

    public override void EnterState(Eagle eagle)
    {
        Vector2 currentPos = eagle.transform.position;
        Vector2 initialPos = eagle.GetInitialPosition();

        if (Vector2.Distance(currentPos, initialPos) > eagle.idleFlyRadius * 0.5f)
        {
            Vector2 randomPointInRadius = initialPos + (Random.insideUnitCircle.normalized * eagle.idleFlyRadius * 0.8f);
            eagle.SetIdleTargetPosition(new Vector3(randomPointInRadius.x, randomPointInRadius.y, eagle.GetInitialPosition().z));
            eagle.SetMovingToInitialIdleTarget(true);
        }
        else
        {
            SetNewIdleDestination(eagle);
            eagle.SetMovingToInitialIdleTarget(false);
        }
    }

    public override void UpdateState(Eagle eagle)
    {
        if (eagle.DistanceToPlayer() < eagle.detectionRange)
        {
            eagle.TransitionToState(eagle.ChasingState);
            return;
        }

        Vector3 currentEaglePos = eagle.transform.position;
        Vector3 targetPosition = eagle.GetIdleTargetPosition();

        targetPosition.z = currentEaglePos.z;

        eagle.transform.position = Vector3.MoveTowards(currentEaglePos, targetPosition, eagle.idleFlySpeed * Time.deltaTime);

        Vector2 directionToTarget = ((Vector2)targetPosition - (Vector2)currentEaglePos).normalized;
        if (directionToTarget != Vector2.zero)
        {
            eagle.FaceDirection2D(directionToTarget);
        }

        if (Vector2.Distance((Vector2)currentEaglePos, (Vector2)targetPosition) < 0.1f)
        {
            if (eagle.IsMovingToInitialIdleTarget())
            {
                eagle.SetMovingToInitialIdleTarget(false);
            }
            SetNewIdleDestination(eagle);
        }
    }

    private void SetNewIdleDestination(Eagle eagle)
    {
        Vector2 initialPos = eagle.GetInitialPosition();
        float randomExtent = eagle.idleFlyRadius * Random.Range(0.7f, 1f);

        Vector2 offset = (movingRight ? Vector2.right : Vector2.left) * randomExtent;
        offset.y += Random.Range(-eagle.idleFlyRadius * 0.1f, eagle.idleFlyRadius * 0.1f);

        Vector2 newTarget2D = initialPos + offset;
        eagle.SetIdleTargetPosition(new Vector3(newTarget2D.x, newTarget2D.y, eagle.GetInitialPosition().z));
        movingRight = !movingRight;
    }

    public override void ExitState(Eagle eagle)
    {
    }
}