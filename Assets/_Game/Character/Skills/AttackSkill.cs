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

    void Update()
    {
        var stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        
        this.IsRunning = stateInfo.IsName(AnimatorAttackVar);
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
                var dmgDlr = effect.GetComponent<DamageDealer>();
                dmgDlr.AddDamage(dmg);
                dmgDlr.SourceTag = this.SourceTag;

                var frcDlr = effect.GetComponent<ForceDealer>();
                frcDlr.Force = SkillData.Force;
                frcDlr.SourceTag = this.SourceTag;
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
