using UnityEngine;

public class Eagle : Enemy
{
    [Header("References")]
    public Transform player;
    public Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Ranges & Speeds")]
    public float detectionRange = 15f;
    public float attackRange = 10f;
    public float moveSpeed = 5f;
    public float idleFlyRadius = 4f;
    public float idleFlySpeed = 1.5f;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    private EagleBaseState currentState;
    private Vector3 initialPosition;
    private Vector3 idleTargetPosition;
    private bool movingToInitialIdleTarget = true;
    private bool isDead = false;
    private float lastShootTime = -Mathf.Infinity;

    private Rigidbody2D rb2D;
    private Collider2D col2D;

    public readonly EagleIdleState IdleState = new EagleIdleState();
    public readonly EagleChasingState ChasingState = new EagleChasingState();
    public readonly EagleAttackingState AttackingState = new EagleAttackingState();
    public readonly EagleDeathState DeathState = new EagleDeathState();

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        initialPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();

        if (animator == null) animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
            else
            {
                enabled = false;
                return;
            }
        }

        if (firePoint == null) firePoint = transform;

        if (projectilePrefab != null && projectilePrefab.scene.IsValid())
        {
            projectilePrefab.SetActive(false);
        }

        TransitionToState(IdleState);
    }

    void Update()
    {
        if (isDead)
        {
            if (currentState != null) currentState.UpdateState(this);
            return;
        }

        if (currentHealth <= 0)
        {
            TransitionToState(DeathState);
            return;
        }

        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void TransitionToState(EagleBaseState state)
    {
        if (isDead && state != DeathState) return;

        if (currentState != null) currentState.ExitState(this);
        currentState = state;

        if (currentState != null)
        {
            currentState.EnterState(this);
            if (state == DeathState) isDead = true;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0) currentHealth = 0;
    }

    public float DistanceToPlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector2.Distance((Vector2)transform.position, (Vector2)player.position);
    }

    public void FaceDirection2D(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

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

    public Vector3 GetInitialPosition() => initialPosition;
    public void SetIdleTargetPosition(Vector3 target) => idleTargetPosition = target;
    public Vector3 GetIdleTargetPosition() => idleTargetPosition;
    public bool IsMovingToInitialIdleTarget() => movingToInitialIdleTarget;
    public void SetMovingToInitialIdleTarget(bool value) => movingToInitialIdleTarget = value;

    public bool CanShoot()
    {
        return Time.time >= lastShootTime + Mathf.Max(attackCooldown, 0.5f);
    }

    public void Shoot()
    {
        if (isDead || player == null) return;
        if (!CanShoot()) return;

        if (projectilePrefab != null && firePoint != null)
        {
            lastShootTime = Time.time;

            Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)firePoint.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion projectileRotation = Quaternion.Euler(0, 0, angle);

            GameObject projectileObj = Instantiate(projectilePrefab, (Vector2)firePoint.position, projectileRotation);
            projectileObj.SetActive(true);

            Collider2D projectileCol = projectileObj.GetComponent<Collider2D>();
            if (projectileCol != null)
            {
                projectileCol.usedByEffector = false;
                projectileCol.isTrigger = true;
                if (col2D != null)
                {
                    Physics2D.IgnoreCollision(projectileCol, col2D);
                }
            }

            if (projectileObj.GetComponent<Projectile2D>() == null)
            {
                projectileObj.AddComponent<Projectile2D>();
            }

            Rigidbody2D projectileRb = projectileObj.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.linearVelocity = directionToPlayer * projectileSpeed;
            }
        }
    }

    public Rigidbody2D GetRigidbody2D() => rb2D;
    public Collider2D GetCollider2D() => col2D;

    void OnDrawGizmosSelected()
    {
        if (isDead) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (Application.isPlaying && currentState is EagleIdleState)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, idleTargetPosition);
            Gizmos.DrawWireSphere(initialPosition, idleFlyRadius);
        }
    }
}