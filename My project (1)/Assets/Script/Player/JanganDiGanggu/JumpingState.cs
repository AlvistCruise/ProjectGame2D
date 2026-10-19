using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public JumpingState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        player.GetAnimator().SetInteger("state", 2);
        player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        if (player.rb.linearVelocity.y < 0)
            player.ChangeState(new FallingState(player));
    }
}
