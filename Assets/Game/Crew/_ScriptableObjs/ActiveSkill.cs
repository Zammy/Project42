using UnityEngine;
using System.Collections;

public enum ActiveSkillType
{
    AOESlowdown,
    Blink,
    Shield,
    FakeTarget
}

[CreateAssetMenuAttribute(fileName="ActiveSkill", menuName="Character/ActiveSkill")]
public class ActiveSkill : ScriptableObject 
{
    public string Name;
    public ActiveSkillType Type;
    public int Recharges;
    public float CastSpeed;
}
