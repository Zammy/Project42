using UnityEngine;
using System.Collections;
using System;

public class Brawler_Punch : Skill
{
    //
    public Animator Animator;
    //

    private new MeleeAttackSkillData SkillData
    {
        get
        {
            return base.SkillData as MeleeAttackSkillData;
        }
    }

    public override void Activate()
    {
        Animator.SetBool("Punch", true);

        //if (this.IsExecuting)
        //{
        //    return;
        //}

        //this.IsExecuting = true;

        //StartCoroutine(DoExecute());
    }

    public override void Deactivate()
    {
        Animator.SetBool("Punch", false);
    }

    //IEnumerator DoExecute()
    //{
    //    Animator.SetTrigger("Punch");

    //    yield return StartCoroutine(Animator.xWaitWhileState("RightPunch"));

    //    this.IsExecuting = false;
    //}

}
