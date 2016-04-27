using UnityEngine;

[CreateAssetMenuAttribute(fileName="Character", menuName="Character/Info")]
public class CharacterInfo : ScriptableObject 
{
    public string Name;

    public float Speed;
    public float Accuracy;
    public int HP;

    public GameObject Weapon; //prefab

    public ActiveSkill ActiveSkill;
    public PassiveSkill PassiveSkill;
}
