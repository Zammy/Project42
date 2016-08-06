using UnityEngine;
using System.Collections;
using System;

public class Brawler_Punch : AttackSkill
{
    //
    public GameObject LeftFistFire;
    public GameObject RightFistFire;
    //

    void Awake()
    {
        this.AnimatorAttackVar = "Punch";
    }

    protected override void Start()
    {
        base.Start();

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
}
