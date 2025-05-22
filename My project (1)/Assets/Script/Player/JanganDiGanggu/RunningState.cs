using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : PlayerState
{
    public RunningState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        player.GetAnimator().SetInteger("state", (int)1);
    }

    public override void HandleInput()
    {
        float hDirection = Input.GetAxisRaw("Horizontal");
        // player.rb.velocity = new Vector2(hDirection * player.speed, player.rb.velocity.y);

        // if (hDirection == 0)
        //     player.ChangeState(new IdleState(player));
        // if (Input.GetButtonDown("Jump") && player.IsGrounded())
        //     player.ChangeState(new JumpingState(player));

        if (hDirection < 0)
        {
            player.rb.velocity = new Vector2(-player.speed, player.rb.velocity.y);
            player.transform.localScale = new Vector2(-1, 1);
        }
        else if (hDirection > 0)
        {
            player.rb.velocity = new Vector2(player.speed, player.rb.velocity.y);
            player.transform.localScale = new Vector2(1, 1);
        }
        else if (Input.GetButtonUp("Horizontal")) // && isJumping == false
        {
            player.rb.velocity = new Vector2(0, player.rb.velocity.y);
            player.ChangeState(new IdleState(player));
        }

        if (Input.GetButtonDown("Jump") && player.IsGrounded())
        {
            player.ChangeState(new JumpingState(player));
            // player.IsGrounded();
        }
    }
}
