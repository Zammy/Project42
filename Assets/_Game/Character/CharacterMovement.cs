using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
    //Set through Unity
    public Animator Animator;
    //

    public Vector3 MovementDirection { get; set; }

    protected virtual void FixedUpdate()
    {
        if (MovementDirection.sqrMagnitude < 0.5f)
        {
            this.Animator.SetBool("Moving", false);
            return;
        }

        this.Animator.SetBool("Moving", true);

//        if (body.velocity.magnitude > MoveMode.MaxSpeed)
//        {
//            body.velocity = body.velocity.normalized * MoveMode.MaxSpeed;
//        }

        transform.localRotation = Quaternion.LookRotation(MovementDirection);
    }
}
