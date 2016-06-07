using UnityEngine;
using System.Collections;

public class HeatWeapon : Weapon 
{
    public float HeatPerShot;
    public float CoolingPerSecond;
    public SpriteRenderer HeatSprite;

    public override bool IsActive
    {
        get
        {
            return base.IsActive;
        }
        set
        {
            if (this.heat >= 1f)
            {
                value = false;
            }
            base.IsActive = value;
        }
    }

    public float heat = 0f;
    public float Heat
    {
        get
        {
            return heat;
        }
        set
        {
            //punishment
//            if(value > 1f && heat < 1f)
//            {
//                value += 1f;
//            }
           
            heat = Mathf.Clamp(value, 0f, float.MaxValue);

            var color = this.HeatSprite.color;
            color.a = Mathf.Clamp01(heat);
            this.HeatSprite.color = color;
        }
    }

    protected override void Update()
    {
        if (this.Heat >= 1)
        {
            this.IsActive = false;
        }

        this.Heat -= CoolingPerSecond * Time.deltaTime;

        base.Update();
    }

    protected override void Shoot()
    {
        base.Shoot();
        Heat += HeatPerShot;
    }

}
