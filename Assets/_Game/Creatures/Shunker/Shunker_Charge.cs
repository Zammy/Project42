using UnityEngine;
using System.Collections;
using System;

public class Shunker_Charge : AIGlobalState
{
    public float ChargeWhenPlayerDistance = 3f;

    public override bool ShouldActivate()
    {
        var activeState = StateManager.ActiveState;

        if (activeState is Shunker_SeekPlayer
            || activeState is Shunker_AttackPlayer
            || activeState is Shunker_ShootPlayer
            || activeState is Dying)
        {
            return false;
        }

        float sqrDistTarget = ChargeWhenPlayerDistance * ChargeWhenPlayerDistance;

        float sqrDist = (PlayerPos - CreatureTransform.position).sqrMagnitude;

        return sqrDistTarget > sqrDist;
    }

    public override void StateUpdate()
    {
        StateManager.ActivateState<Shunker_SeekPlayer>();
    }
}
