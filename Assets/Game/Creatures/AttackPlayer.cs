using System;
using System.Collections;
using UnityEngine;

public abstract class AttackPlayer : AIState
{
//    public Weapon Weapon;
//    public Animator Animator;

    public int SkillIndex;

    CharacterSkills charSkills;

    new Rigidbody rigidbody;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        charSkills = CreatureObject.GetComponent<CharacterSkills>();

        CreatureTransform.LookAt(PlayerPos);

        charSkills.ExecuteSkill(SkillIndex);

        rigidbody = CreatureObject.GetComponent<Rigidbody>();
        rigidbody.drag *= 2f;

//        this.Animator.SetTrigger("Attack");
    }

    public override void OnExit(AIState nextState)
    {
        base.OnExit(nextState);

//        this.Weapon.IsActive = false;
    }

    public override void StateUpdate()
    {
        if (!charSkills.CastingSkill)
        {
            rigidbody.drag /= 2f;
            this.OnAttackFinished();
        }

//        AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);

//        bool isAttacking = info.IsName("Attack");
//        if (!isAttacking && this.Weapon.IsActive && info.IsName("Default"))
//        {
//            this.OnAttackFinished();
//        }
//        this.Weapon.IsActive = isAttacking;
    }

    protected abstract void OnAttackFinished();
}
