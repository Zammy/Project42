using UnityEngine;
using System.Collections;

public class Shunker_AttackPlayer : AttackPlayer 
{
    public int NumOfAttacks = 3;

    int attacks;

    public override void OnEnter(AIState previousState)
    {
        attacks = 0;

        base.OnEnter(previousState);
    }

    protected override void OnAttackFinished()
    {
        attacks += 1;
        if (attacks < NumOfAttacks)
        {
            Attack();
            return;
        }
        StateManager.ActivateState<Shunker_Wonder>();
    }
}
