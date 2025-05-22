using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    private Collider2D coll;
    private PlayerState currentState;

    [SerializeField] public float speed = 8f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] private LayerMask ground;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        ChangeState(new IdleState(this)); // Initialize with Idle
    }

    void Update()
    {
        currentState.HandleInput();
        currentState.Update();
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public bool IsGrounded()
    {
        return coll.IsTouchingLayers(ground);
    }

    public Animator GetAnimator()
    {
        return anim;
    }
}

