using UnityEngine;
using System.Collections;

[CreateAssetMenuAttribute(fileName = "DodgeSkill", menuName = "Character/DodgeSkill")]
public class DodgeSkillData : SkillData
{
    public float InvurnabilityBegin;
    public float InvurnabilityEnd;

    public string AnimatorTrigger;
}
