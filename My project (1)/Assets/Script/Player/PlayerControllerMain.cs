using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerMain : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask ground;
    private bool isJumping = false;

    [SerializeField] private int hurtForce = 10;

    [SerializeField] private int cherries = 0;
    [SerializeField] private int health = 5;

    [SerializeField] private Text cherriesTXT;
    [SerializeField] private Text healthTXT;

    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballSpawnPoint;
    [SerializeField] private float fireballSpeed = 10f;
    [SerializeField] private float fireCooldown = 3;
    [SerializeField] private Text fireCooldownTXT;

    #region FiniteStateMachine
    private enum State
    { idle, running, jumping, falling, hurt }
    private State state = State.idle;
    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        healthTXT.text = health.ToString();
    }

    // Update is called once per frame
    private void FixedUpdate()
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

        // if (coll.IsTouchingLayers(ground) == true)
        // {
        //     isJumping = false;
        // }
        if (fireCooldown > 0)
        {
            fireCooldownTXT.text = fireCooldown.ToString();
            fireCooldown -= Time.deltaTime;
        }
        else
        {
            fireCooldownTXT.text = fireCooldown.ToString();
            fireCooldown = 0;
        }

        if (Input.GetKeyDown(KeyCode.E) && fireCooldown == 0)
            {
                fireCooldown = 3;
                fireCooldownTXT.text = fireCooldown.ToString();
                ShootFireball();
            }
    }


    private void Movement()
    {
        float hDirection = Input.GetAxisRaw("Horizontal");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else if (Input.GetButtonUp("Horizontal") && isJumping == false) // && isJumping == false
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
        isJumping = true;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .3f)
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
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
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
            cherries += 1;
            cherriesTXT.text = cherries.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            FrogController frog = other.gameObject.GetComponent<FrogController>();
            if (state == State.falling)
            {
                frog.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    health -= 1;
                    healthTXT.text = health.ToString();
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    health -= 1;
                    healthTXT.text = health.ToString();
                }
            }
        }
    }





    private void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(fireballSpeed * transform.localScale.x, 0);
        fireball.transform.localScale = new Vector2(transform.localScale.x, 1);

        Destroy(fireball, 2.5f);
    }


}
