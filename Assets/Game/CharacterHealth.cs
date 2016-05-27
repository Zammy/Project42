﻿using DG.Tweening;
using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour 
{
    public int Health;
    public SpriteRenderer SpriteToFlash;

    public event Action<GameObject> CharacterDied;

    void OnDestroy()
    {
        Level.Instance.RemoveCreature(this.gameObject);
    }

    public void DealDamage(int damage)
    {
        DOTween.Sequence()
            .Append(this.SpriteToFlash.DOColor(Color.red, .15f))
            .Append(this.SpriteToFlash.DOColor(Color.white, .15f));

        this.Health -= damage;

        if (this.Health <= 0)
        {
            this.RaiseOnCharacterDied();
        }
    }

    void RaiseOnCharacterDied()
    {
        if (this.CharacterDied != null)
        {
            this.CharacterDied(this.gameObject);
        }
    }
}
