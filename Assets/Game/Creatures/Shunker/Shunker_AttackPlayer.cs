using UnityEngine;
using System.Collections;

public class Shunker_AttackPlayer : AttackPlayer 
{
    protected override void OnAttackFinished()
    {
        StateManager.ActivateState<Shunker_Wonder>();
    }
}
