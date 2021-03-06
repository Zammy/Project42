﻿using System;
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

        this.Weapon.IsActive = false;
    }

    public override void StateUpdate()
    {
        AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);

        bool isAttacking = info.IsName("Attack");
        if (!isAttacking && this.Weapon.IsActive && info.IsName("Default"))
        {
            this.OnAttackFinished();
        }
        this.Weapon.IsActive = isAttacking;
    }

    protected abstract void OnAttackFinished();
}
