// EagleIdleState.cs (2D Version)
using UnityEngine;

public class EagleIdleState : EagleBaseState
{
    private bool _movingRight = true;

    public override void EnterState(Eagle eagle)
    {
        Debug.Log("Eagle entering Idle State (2D).");
        Vector2 currentPos = eagle.transform.position; // Use Vector2 for logical position
        Vector2 initialPos = eagle.GetInitialPosition();

        if (Vector2.Distance(currentPos, initialPos) > eagle.idleFlyRadius * 0.5f)
        {
            // For 2D, Random.insideUnitCircle gives a point in XY plane
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
        Vector3 targetPosition = eagle.GetIdleTargetPosition(); // This is still Vector3 to match transform.position

        // Ensure Z position remains constant if not intentionally changed
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
        Vector2 initialPos = eagle.GetInitialPosition(); // Get initial position as Vector2
        float randomExtent = eagle.idleFlyRadius * Random.Range(0.7f, 1f);
        
        // For 2D, offset mainly on X, can add slight Y variation
        Vector2 offset = (_movingRight ? Vector2.right : Vector2.left) * randomExtent;
        offset.y += Random.Range(-eagle.idleFlyRadius * 0.1f, eagle.idleFlyRadius * 0.1f); // Smaller Y variation

        Vector2 newTarget2D = initialPos + offset;
        // Convert back to Vector3, preserving original Z
        eagle.SetIdleTargetPosition(new Vector3(newTarget2D.x, newTarget2D.y, eagle.GetInitialPosition().z));
        _movingRight = !_movingRight;
    }

    public override void ExitState(Eagle eagle)
    {
        Debug.Log("Eagle exiting Idle State (2D).");
    }
}