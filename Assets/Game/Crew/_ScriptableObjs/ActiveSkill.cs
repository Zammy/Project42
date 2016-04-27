using UnityEngine;
using System.Collections;

public enum ActiveSkillType
{
    AOESlowdown,
    Blink
}

[CreateAssetMenuAttribute(fileName="ActiveSkill", menuName="Character/ActiveSkill")]
public class ActiveSkill : ScriptableObject 
{
    public string Name;
    public ActiveSkillType Type;
    public int Recharges;
    public float CastSpeed;
}
