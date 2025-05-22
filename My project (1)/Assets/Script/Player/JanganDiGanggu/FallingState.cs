using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : PlayerState
{
    public FallingState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        player.GetAnimator().SetInteger("state", 3);
    }

    public override void Update()
    {
        if (player.IsGrounded())
            player.ChangeState(new IdleState(player));
    }
}
