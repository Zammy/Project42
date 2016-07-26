using UnityEngine;
using System.Collections;
using System;

public class Brawler_Punch : Skill
{
    //
    public Animator Animator;
    public GameObject LeftFistFire;
    public GameObject RightFistFire;
    //

    void Update()
    {
        var animState = Animator.GetCurrentAnimatorStateInfo(0);
        bool activate = animState.IsName("Punch");
        LeftFistFire.SetActive(activate);
        RightFistFire.SetActive(activate);
    }

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
    }

    public override void Deactivate()
    {
        Animator.SetBool("Punch", false);
    }
}
