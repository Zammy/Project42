using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class MoveTowardObject : Action 
{
    //Set through Unity
    public Animator Animator;
    public SharedGameObject Target;

    public float Distance;
    //

    Vector3 movementDirection { get; set; }


    public override TaskStatus OnUpdate()
    {
        var diff = (Target.Value.transform.position - this.transform.position);

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
