using DG.Tweening;
using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour 
{
    public int Health;
    public MeshRenderer Renderer;

    public float KineticDamage_Multiplier;
    public float FireDamage_Multiplier;
    public float ElectricalDamage_Multiplier;

    public event Action<Damage> DealtDamage;
    public event Action<GameObject> CharacterDied;

    public void DealDamage(Damage damage)
    {
        if (this.Renderer != null)
        {
            var flashColor = Color.red;
            var originalColor = Renderer.material.color;

            DOTween.Sequence()
                .Append(this.Renderer.material.DOColor(flashColor, .15f))
                .Append(this.Renderer.material.DOColor(originalColor, .15f));
        }

        float multiplier = KineticDamage_Multiplier;
        if (damage.Type == DamageType.Fire)
            multiplier = FireDamage_Multiplier;
        else if (damage.Type == DamageType.Electrical)
            multiplier = ElectricalDamage_Multiplier;

        this.Health -= Mathf.RoundToInt( damage.Amount * multiplier );
        this.RaiseOnDealtDamage(damage);

        if (this.Health <= 0)
        {
            this.RaiseOnCharacterDied();
        }

        InGameUI.Instance.ShowDamageLabel(transform.position, damage.Amount);
    }

    void RaiseOnCharacterDied()
    {
        if (this.CharacterDied != null)
        {
            this.CharacterDied(this.gameObject);
        }
    }

    void RaiseOnDealtDamage(Damage dmg)
    {
        if (this.DealtDamage != null)
        {
            this.DealtDamage(dmg);
        }
    }
}
