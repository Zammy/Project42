using UnityEngine;
using System.Collections;
using System;

public class Attack : AIState
{
    public Weapon Weapon;
    public Animator Animator;

    public override void OnEnter()
    {
        throw new NotImplementedException();
    }

    public override void OnExit()
    {
        throw new NotImplementedException();
    }

    void FixedUpdate()
    {
        AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);

        this.Weapon.IsActive = info.IsName("Attack");
    }
}
