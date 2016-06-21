using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CharacterSkills : MonoBehaviour 
{
    public SkillData[] Skills;
    public GameObject AttackHitEffectPrefab;
    public Transform Face;

    public bool CastingSkill
    {
        get;
        set;
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
        
        this.CastingSkill = true;

        yield return new WaitForSeconds(skillData.CastTime);

        var attackSkill = skillData as AttackSkillData ; 
        if (attackSkill != null)
        {
            var instantiatePos = Face.transform.position + Face.transform.forward * attackSkill.Depth/2;
            var attackHitEffect = (GameObject) Instantiate( AttackHitEffectPrefab, instantiatePos, Quaternion.identity);
            attackHitEffect.transform.localScale = new Vector3(attackSkill.Breadth, 1, attackSkill.Depth);

            var hitEffect = attackHitEffect.GetComponent<HitEffect>();
            hitEffect.Life = attackSkill.AttackTime;

            attackHitEffect.transform.localRotation = Quaternion.LookRotation(this.transform.forward);
        }

        yield return new WaitForSeconds(skillData.WindTime);

        this.CastingSkill = false;
    }
}
