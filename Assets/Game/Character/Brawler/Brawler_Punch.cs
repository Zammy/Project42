using UnityEngine;
using System.Collections;
using System;

public class Brawler_Punch : AttackSkill
{
    //
    public Animator Animator;
    public GameObject LeftFistFire;
    public GameObject RightFistFire;
    //

    void Start()
    {
        var fists = new GameObject[] { LeftFistFire, RightFistFire };

        this.LoadSkillData(fists);
    }

    void Update()
    {
        var animState = Animator.GetCurrentAnimatorStateInfo(0);
        bool activate = animState.IsName("Punch");
        LeftFistFire.SetActive(activate);
        RightFistFire.SetActive(activate);
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
