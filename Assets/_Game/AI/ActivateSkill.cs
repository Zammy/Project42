using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;

public class ActivateSkill : Action 
{
    //Set through Unity
    public GameObject Skill;
    //

    Skill skill;

    public override void OnStart()
    {
        base.OnStart();

        skill = Skill.GetComponent<Skill>();

        skill.Activate();
    }

    public override TaskStatus OnUpdate()
    {
        if (skill.IsRunning)
        {
            skill.Deactivate();
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }
}
