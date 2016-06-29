using UnityEngine;
using System.Collections;

public class Shunker_ShootPlayer : AttackPlayer 
{
    protected override void OnAttackFinished()
    {
        StateManager.ActivateState<Shunker_Wonder>();
    }
}
