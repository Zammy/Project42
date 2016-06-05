using DG.Tweening;
using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour 
{
    public int Health;
    public SpriteRenderer SpriteToFlash;

    public event Action<GameObject> CharacterDied;

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

        InGameUI.Instance.ShowDamageLabel(transform.position, this.Health);
    }

    void RaiseOnCharacterDied()
    {
        if (this.CharacterDied != null)
        {
            this.CharacterDied(this.gameObject);
        }
    }
}
