using UnityEngine;
using System.Collections;

public class Punker_AttackPlayer : AttackPlayer 
{
    protected override void OnAttackFinished()
    {
        StateManager.ActivateState<Punker_Wonder>();
    }
}
