using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MoveTowardPosition : Action 
{

    //Set through Unity
    public Animator Animator;
    public SharedVector3 Target;

    public float Distance;
    //

    Vector3 movementDirection { get; set; }


    public override TaskStatus OnUpdate()
    {
        var diff = (Target.Value - this.transform.position);

        if (diff.sqrMagnitude < Distance * Distance)
        {
            this.Animator.SetBool("Moving", false);
            return TaskStatus.Success;
        }

        movementDirection = diff.normalized;

        this.Animator.SetBool("Moving", true);

        transform.localRotation = Quaternion.LookRotation(movementDirection);

        return TaskStatus.Running;
    }
}
