using UnityEngine;
using System.Collections;
using System;

public class DogAttack : Attack
{
    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        CreatureTransform.transform.xLookAt(this.CrewPos);
    }

    protected override void OnAttackFinished()
    {
        StateManager.ActivateState<DogAvoidCrew>();
    }
}
