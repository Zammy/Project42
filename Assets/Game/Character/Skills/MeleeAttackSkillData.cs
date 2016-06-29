using UnityEngine;
using System.Collections;

[CreateAssetMenuAttribute(fileName="MeleeAttack", menuName="Character/MeleeAttack")]
public class MeleeAttackSkillData : AttackSkillBaseData
{
    public float AttackTime;
    public float Breadth;
    public float Depth;
}