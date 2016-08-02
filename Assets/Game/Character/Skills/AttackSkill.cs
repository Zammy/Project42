using UnityEngine;
using System.Collections;

public abstract class  AttackSkill : Skill 
{

    private new MeleeAttackSkillData SkillData
    {
        get
        {
            return base.SkillData as MeleeAttackSkillData;
        }
    }

    protected void LoadSkillData(GameObject effect)
    {
        LoadSkillData(new GameObject[] { effect });
    }

    protected void LoadSkillData(GameObject[] effects)
    {
        foreach (var dmg in SkillData.Damage)
        {
            foreach(var effect in effects)
            {
                effect.GetComponent<DamageDealer>().AddDamage(dmg);
                effect.GetComponent<ForceDealer>().Force = SkillData.Force;
            }
        }
    }
}
