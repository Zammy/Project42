using System;
using System.Collections;
using UnityEngine;

public abstract class Attack : AIState
{
    public Weapon Weapon;
    public Animator Animator;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        this.Animator.SetTrigger("Attack");
    }

    public override void OnExit(AIState nextState)
    {
        base.OnExit(nextState);
    }

    public override void StateUpdate(AIStateManager mng)
    {
        AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);

        bool isAttacking = info.IsName("Attack");
        if (!isAttacking && this.Weapon.IsActive && info.IsName("Default"))
        {
            this.OnAttackFinished(mng);
        }
        this.Weapon.IsActive = isAttacking;
    }

    protected abstract void OnAttackFinished(AIStateManager mng);
}
