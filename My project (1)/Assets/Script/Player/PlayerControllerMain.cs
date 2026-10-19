using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMain : PlayerAttributes
{
    #region FiniteStateMachine
    private enum State
    { idle, running, jumping, falling, hurt }
    private State state = State.idle;
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (Time.deltaTime != 0)
        {
            if (state != State.hurt)
            {
                Movement();
            }
            AnimationState();
            anim.SetInteger("state", (int)state);
        }

        UpdateFireCooldown();

        if (Input.GetKeyDown(KeyCode.E) && fireCooldown == 0)
        {
            ResetFireCooldown(3f);
            ShootFireball();
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxisRaw("Horizontal");

        if (hDirection < 0)
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hDirection > 0)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else if (Input.GetButtonUp("Horizontal") && isJumping == false)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        state = State.jumping;
        isJumping = true;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.linearVelocity.y < .3f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
                isJumping = false;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.linearVelocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.linearVelocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            AddCherry(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.linearVelocity = new Vector2(-hurtForce, rb.linearVelocity.y);
                    TakeDamage(1);
                }
                else
                {
                    rb.linearVelocity = new Vector2(hurtForce, rb.linearVelocity.y);
                    TakeDamage(1);
                }
            }
        }
    }

    private void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(fireballSpeed * transform.localScale.x, 0);
        fireball.transform.localScale = new Vector2(transform.localScale.x, 1);

        Destroy(fireball, 2.5f);
    }
}
