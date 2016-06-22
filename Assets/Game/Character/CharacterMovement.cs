using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
    //Set through Unity
    public float Speed;
    public Animator Animator;
    //

    CharacterSkills charSkills;

    public Vector3 MovementDirection
    {
        get;
        set;
    }

    protected virtual void Start()
    {
        charSkills = GetComponent<CharacterSkills>();
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

        var move = MovementDirection * (this.Speed * Time.deltaTime);
        transform.position += move;

        transform.localRotation = Quaternion.LookRotation(MovementDirection);
    }
}
