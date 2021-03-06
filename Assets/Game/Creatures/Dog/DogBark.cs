﻿using UnityEngine;
using System.Collections;
using System;

public class DogBark : AIState
{
    public Animator Animator;
    public Transform HeadTrans;
    public GameObject WoofPrefab;

    bool hasBarked = false;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);
        hasBarked = false;
        Animator.SetTrigger("Bark");

        CreatureTransform.transform.xLookAt(this.CrewPos);
    }

    public override void StateUpdate()
    {
        AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);

        bool isBarking = info.IsName("Bark");
        if (isBarking && !hasBarked)
        {
            hasBarked = true;
            GameObject woof = (GameObject)Instantiate(WoofPrefab);
            woof.transform.position = HeadTrans.position;
        }

        if (!isBarking && info.IsName("Default") && hasBarked)
        {
            StateManager.ActivateState<DogSeekCrew>();
        }
    }
}
