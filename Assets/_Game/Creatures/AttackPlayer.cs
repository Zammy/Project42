using System;
using System.Collections;
using UnityEngine;

public abstract class AttackPlayer : AIState
{
    public int SkillIndex;

    CharacterSkills charSkills;

    Rigidbody body;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        charSkills = CreatureObject.GetComponent<CharacterSkills>();

        Attack();

        body = CreatureObject.GetComponent<Rigidbody>();
        body.drag *= 2f;
    }

    public override void OnExit(AIState nextState)
    {
        base.OnExit(nextState);
        body.drag /= 2f;
    }

    public override void StateUpdate()
    {
        if (!charSkills.CastingSkill)
        {
            this.OnAttackFinished();
        }
    }

    protected void Attack()
    {
        CreatureTransform.LookAt(PlayerPos);
        charSkills.ExecuteSkill(SkillIndex);
    }

    protected abstract void OnAttackFinished();
}
