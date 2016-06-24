using UnityEngine;
using System.Collections;

[CreateAssetMenuAttribute(fileName="AttackSkill", menuName="Character/AttackSkill")]
public class AttackSkillData : SkillData 
{
    public Damage[] Damage;
    public float AttackTime;
    public float Breadth;
    public float Depth;

    public float Force;
}