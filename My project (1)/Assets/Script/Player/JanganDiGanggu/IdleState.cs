using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        player.GetAnimator().SetInteger("state", 0);
        player.rb.velocity = Vector2.zero;
    }

    public override void HandleInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
            player.ChangeState(new RunningState(player));
        if (Input.GetButtonDown("Jump") && player.IsGrounded())
            player.ChangeState(new JumpingState(player));
    }
}
