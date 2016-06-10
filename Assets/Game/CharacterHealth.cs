using DG.Tweening;
using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour 
{
    public int Health;
    public SpriteRenderer SpriteToFlash;

    public event Action<int> DealtDamage;
    public event Action<GameObject> CharacterDied;

    public bool IsShielded { get; set; }

    public void DealDamage(int damage, Vector2 projectileDir)
    {
        float dot = Vector2.Dot(projectileDir, this.transform.up.xToVector2());
        bool shielded = IsShielded && dot < 0;

        if (this.SpriteToFlash != null)
        {
            var flashColor = Color.red;
            if (shielded)
            {
                flashColor = Color.blue;
            }

            DOTween.Sequence()
                .Append(this.SpriteToFlash.DOColor(flashColor, .15f))
                .Append(this.SpriteToFlash.DOColor(Color.white, .15f));
        }

        if (shielded)
        {
            return;
        }

        this.Health -= damage;
        this.RaiseOnDealtDamage(damage);

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

    void RaiseOnDealtDamage(int dmg)
    {
        if (this.DealtDamage != null)
        {
            this.DealtDamage(dmg);
        }
    }
}
