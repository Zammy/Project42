using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
    //Set through Unity
    public float MaxSpeed;
    public float Acceleration;
    public Animator Animator;
    //

    CharacterSkills charSkills;
    Rigidbody body;

    public Vector3 MovementDirection
    {
        get;
        set;
    }

    protected virtual void Start()
    {
        charSkills = GetComponent<CharacterSkills>();
        body = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (MovementDirection.sqrMagnitude < 0.5f || 
            charSkills.CastingSkill)
        {
            this.Animator.SetBool("Moving", false);
            return;
        }

        this.Animator.SetBool("Moving", true);

        //var move = MovementDirection * (this.Speed * Time.deltaTime);
        //transform.position += move;

        body.AddForce(this.MovementDirection * Acceleration);

        if (body.velocity.magnitude > MaxSpeed)
        {
            body.velocity = body.velocity.normalized * MaxSpeed;
        }

        transform.localRotation = Quaternion.LookRotation(MovementDirection);
    }
}
