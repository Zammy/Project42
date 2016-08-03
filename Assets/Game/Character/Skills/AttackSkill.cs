using UnityEngine;
using System.Collections;

public abstract class  AttackSkill : Skill 
{
    //Set through Unity
    public Animator Animator;
    //

    protected string AnimatorAttackVar
    {
        get;
        set;
    }

    private new MeleeAttackSkillData SkillData
    {
        get
        {
            return base.SkillData as MeleeAttackSkillData;
        }
    }

    protected GameObject InstantiateEffect(GameObject prefab, Transform trans)
    {
        var effect = (GameObject) Instantiate(prefab);
        effect.transform.SetParent(trans.parent);
        effect.transform.xCloneTransformFrom(trans);
        effect.SetActive(false);
        return effect;
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

    public override void Activate()
    {
        Animator.SetBool(AnimatorAttackVar, true);
    }

    public override void Deactivate()
    {
        Animator.SetBool(AnimatorAttackVar, false);
    }
}
