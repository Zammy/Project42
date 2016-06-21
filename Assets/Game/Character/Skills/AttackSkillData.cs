using UnityEngine;
using System.Collections;

[CreateAssetMenuAttribute(fileName="AttackSkill", menuName="Character/AttackSkill")]
public class AttackSkillData : SkillData 
{
    public int Damage;
    public float AttackTime;
    public float Breadth;
    public float Depth;
}