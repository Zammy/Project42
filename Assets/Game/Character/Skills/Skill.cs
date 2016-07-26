using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public SkillData SkillData;

    public abstract void Activate();
    public abstract void Deactivate();


    public bool IsExecuting { get; protected set; }
}
