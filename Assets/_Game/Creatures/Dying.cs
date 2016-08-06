﻿using UnityEngine;
using System.Collections;
using System;

public class Dying : AIGlobalState
{
    public Animator Animator;
    public CharacterHealth CharacterHealth;

    public Collider[] Colliders;

    void Start()
    {
        CharacterHealth.CharacterDied += this.OnCharacterDied;

    }

    void OnDestroy()
    {
        CharacterHealth.CharacterDied -= this.OnCharacterDied;
    }

    public override bool ShouldActivate()
    {
        var animatorInfo = Animator.GetCurrentAnimatorStateInfo(0);
        return animatorInfo.IsName("Die") && !animatorInfo.IsName("Died");
    }

    public override void StateUpdate()
    {
        var animatorInfo = Animator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsName("Died"))
        {
            //TODO died triggers
            Destroy(CreatureTransform.gameObject);
        }
    }

    private void OnCharacterDied(GameObject _)
    {        
        for (int i = 0; i < Colliders.Length; i++)
        {
            Colliders[i].enabled = false;
        }
        this.Animator.SetTrigger("Die");
//        Level.Instance.RemoveCreature(this.CreatureObject);
    }
}