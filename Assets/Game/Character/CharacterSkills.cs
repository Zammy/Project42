using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CharacterSkills : MonoBehaviour 
{
    public SkillData[] Skills;
    public GameObject AttackHitEffectPrefab;
    public Transform Face;

    CharacterHealth charHealth;

    public bool CastingSkill
    {
        get;
        set;
    }

    void Start()
    {
        charHealth = GetComponent<CharacterHealth>();
    }

    public void ExecuteSkill(int index)
    {
        if (Skills.Length <= index)
            return;

        var skillData = Skills[index];

        if (this.CastingSkill)
            return;

        StartCoroutine( ExecuteSkill (skillData) );

    }

    IEnumerator ExecuteSkill(SkillData skillData)
    {
        if (charHealth.Health <= 0)
            yield break;

        this.CastingSkill = true;

        yield return new WaitForSeconds(skillData.CastTime);

        if (charHealth.Health <= 0)
            yield break;

        var attackSkill = skillData as AttackSkillData ; 
        if (attackSkill != null)
        {
            var instantiatePos = Face.transform.position + Face.transform.forward * attackSkill.Depth/2;
            var attackHitEffectGo = (GameObject) Instantiate( AttackHitEffectPrefab, instantiatePos, Quaternion.identity);
            attackHitEffectGo.transform.localScale = new Vector3(attackSkill.Breadth, 1, attackSkill.Depth);

            var hitEffect = attackHitEffectGo.GetComponent<HitEffect>();
            hitEffect.Life = attackSkill.AttackTime;

            var dmgDealer = attackHitEffectGo.GetComponent<DamageDealer>();
            for (int i = 0; i < attackSkill.Damage.Length; i++)
            {
                dmgDealer.AddDamage(attackSkill.Damage[i]);
            }
            dmgDealer.SourceTag = this.gameObject.tag;

            var forceDealer = attackHitEffectGo.GetComponent<ForceDealer>();
            forceDealer.Force = attackSkill.Force;
            forceDealer.SourceTag = this.gameObject.tag;

            attackHitEffectGo.transform.localRotation = Quaternion.LookRotation(this.transform.forward);
        }

        yield return new WaitForSeconds(skillData.WindTime);

        this.CastingSkill = false;
    }
}
