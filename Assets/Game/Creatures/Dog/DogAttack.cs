using UnityEngine;
using System.Collections;
using System;

public class DogAttack : Attack
{
    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        CreatureTransform.transform.xLookAt(Crew.Instance.transform.position);
    }

    protected override void OnAttackFinished()
    {
        StateManager.ActivateState<DogAvoidCrew>();
    }
}
