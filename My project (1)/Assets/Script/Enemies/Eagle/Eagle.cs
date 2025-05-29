// Eagle.cs (2D Version)
using UnityEngine;

public class Eagle : Enemy
{
    [Header("References")]
    public Transform player;
    public Animator animator; // Assign the Eagle's Animator component here
    private SpriteRenderer spriteRenderer; // For flipping or other sprite manipulations

    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Ranges & Speeds")]
    public float detectionRange = 15f;
    public float attackRange = 10f;
    public float moveSpeed = 5f; // Adjusted for typical 2D speeds
    public float idleFlyRadius = 4f;
    public float idleFlySpeed = 1.5f;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    public GameObject projectilePrefab; // Assign a 2D projectile prefab
    public Transform firePoint;        // Point from where to fire (can be child GameObject)
    public float projectileSpeed = 10f; // Speed for the projectile

    // Private internal variables
    private EagleBaseState _currentState;
    private Vector3 _initialPosition; // Still Vector3 as transform.position is Vector3
    private Vector3 _idleTargetPosition;
    private bool _movingToInitialIdleTarget = true;
    private bool _isDead = false;

    // 2D Specific Components
    private Rigidbody2D rb2D;
    private Collider2D col2D;

    // State Instances
    public readonly EagleIdleState IdleState = new EagleIdleState();
    public readonly EagleChasingState ChasingState = new EagleChasingState();
    public readonly EagleAttackingState AttackingState = new EagleAttackingState();
    public readonly EagleDeathState DeathState = new EagleDeathState();

    

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        _initialPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();

        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null) Debug.LogWarning("Eagle: Animator component not found.");
        if (spriteRenderer == null) Debug.LogWarning("Eagle: SpriteRenderer component not found.");


        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
            else
            {
                Debug.LogError("Eagle: Player not found!");
                enabled = false;
                return;
            }
        }

        if (firePoint == null) firePoint = transform;
        if (!gameObject.CompareTag("Enemy"))
        {
            Debug.LogWarning("Eagle GameObject is not tagged as 'Enemy'. Please set the tag.");
        }

        TransitionToState(IdleState);
    }

    void Update()
    {
        if (_isDead)
        {
            if (_currentState != null) _currentState.UpdateState(this);
            return;
        }

        if (currentHealth <= 0)
        {
            TransitionToState(DeathState);
            return;
        }

        if (_currentState != null)
        {
            _currentState.UpdateState(this);
        }
    }

    public void TransitionToState(EagleBaseState state)
    {
        if (_isDead && state != DeathState) return;

        if (_currentState != null) _currentState.ExitState(this);
        _currentState = state;

        if (_currentState != null)
        {
            _currentState.EnterState(this);
            Debug.Log("Eagle transitioned to: " + state.GetType().Name);
            if (state == DeathState) _isDead = true;
        }
        else Debug.LogError("Attempted to transition to a null state!");
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;
        currentHealth -= amount;
        Debug.Log("Eagle took " + amount + " damage. Current health: " + currentHealth);
        if (currentHealth <= 0) currentHealth = 0;
        // Optional: if (animator != null) animator.SetTrigger("Hit");
    }

    public float DistanceToPlayer()
    {
        if (player == null) return Mathf.Infinity;
        // Use Vector2.Distance for 2D plane calculations
        return Vector2.Distance((Vector2)transform.position, (Vector2)player.position);
    }

    // Faces the Eagle towards a 2D direction by rotating on Z or flipping sprite
    public void FaceDirection2D(Vector2 direction)
    {
        if (direction == Vector2.zero) return; // Don't change facing if direction is zero

        // // Option 1: Rotate the sprite to face the direction (smoother for flying)
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // // Assuming sprite faces right by default. If it faces up, subtract 90 from angle.
        // transform.rotation = Quaternion.Euler(0, 0, angle);

        // Option 2: Flip sprite based on X direction (simpler if no rotation animation)
        if (spriteRenderer != null)
        {
            if (direction.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }


    public Vector3 GetInitialPosition() => _initialPosition;
    public void SetIdleTargetPosition(Vector3 target) => _idleTargetPosition = target;
    public Vector3 GetIdleTargetPosition() => _idleTargetPosition;
    public bool IsMovingToInitialIdleTarget() => _movingToInitialIdleTarget;
    public void SetMovingToInitialIdleTarget(bool value) => _movingToInitialIdleTarget = value;

    public void Shoot()
    {
        if (_isDead || player == null) return;

        Debug.Log("Eagle SHOOTS at player (2D)!");
        if (projectilePrefab != null && firePoint != null)
        {
            Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)firePoint.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion projectileRotation = Quaternion.Euler(0, 0, angle);

            GameObject projectileObj = Instantiate(projectilePrefab, (Vector2)firePoint.position, projectileRotation);
            
            // Assuming projectile has a Rigidbody2D and a script to move it
            Rigidbody2D projectileRb = projectileObj.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.velocity = directionToPlayer * projectileSpeed;
            }
            // Example: Add a simple script to projectile to destroy it after time
            // Destroy(projectileObj, 3f); 
        }
    }

    public Rigidbody2D GetRigidbody2D() => rb2D;
    public Collider2D GetCollider2D() => col2D;

    void OnDrawGizmosSelected()
    {
        if (_isDead) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Still useful in 2D
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (Application.isPlaying && _currentState is EagleIdleState)
        {
            Gizmos.color = Color.cyan; // Changed color for visibility
            Gizmos.DrawLine(transform.position, _idleTargetPosition);
            // Draw idle radius as a circle on XY plane
            Gizmos.DrawWireSphere(_initialPosition, idleFlyRadius);
        }
    }
}