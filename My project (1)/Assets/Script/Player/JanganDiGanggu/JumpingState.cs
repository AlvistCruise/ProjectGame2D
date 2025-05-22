using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public JumpingState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        player.GetAnimator().SetInteger("state", 2);
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        if (player.rb.velocity.y < 0)
            player.ChangeState(new FallingState(player));
    }
}
