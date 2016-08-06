using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    //Set through Unity
    public SkillData SkillData;
    public GameObject TagSource;
    //

    public abstract void Activate();
    public abstract void Deactivate();

    public bool IsRunning
    {
        get;
        protected set;
    }

    protected string SourceTag
    {
        get;
        private set;
    }


    protected virtual void Start()
    {
        SourceTag = TagSource.tag;
    }
}
