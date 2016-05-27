﻿using UnityEngine;
using System.Collections;
using System;

public class Dying : AIGlobalState
{
    public Animator Animator;
    public CharacterHealth CharacterHealth;

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

    public override void StateUpdate(AIStateManager mng)
    {
        var animatorInfo = Animator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsName("Died"))
        {
            //TODO died triggers
            Destroy(creatureTransform.gameObject);
        }
    }

    private void OnCharacterDied(GameObject _)
    {
        this.Animator.SetTrigger("Die");
    }
}