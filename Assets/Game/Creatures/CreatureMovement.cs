using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureMovement : CharacterMovement 
{
    //public float AngularSpeed = 360f;

    public Animator Animator;
    public Transform BehaviorsBase;

    AIBehavior[] behaviors;

    List<Vector2[]> interestMaps = new List<Vector2[]>();
    List<Vector2[]> dangerMaps = new List<Vector2[]>();

	void Start () 
    {
        this.behaviors = BehaviorsBase.GetComponents<AIBehavior>();
    }

    protected override void FixedUpdate()
    {
        this.interestMaps.Clear();
        this.dangerMaps.Clear();

        foreach (var beh in behaviors)
        {
            if (beh.isActiveAndEnabled)
            {
                var interest = beh.GetInterest();
                interestMaps.Add(interest);

                var danger = beh.GetDanger();
                dangerMaps.Add(danger);
            }
        }

        Vector2 newDirection = Vector2.zero;
        for (int i = 0; i < interestMaps.Count; i++)
        {
            var interestMap = interestMaps[i];
            for (int y = 0; y < interestMap.Length; y++)
            {
                newDirection += interestMap[y];
            }
        }

        if (newDirection != Vector2.zero)
        {
            for (int i = 0; i < dangerMaps.Count; i++)
            {
                var dangerMap = dangerMaps[i];
                for (int y = 0; y < dangerMap.Length; y++)
                {
                    newDirection += dangerMap[y];
                }
            }
        }

        if (newDirection.sqrMagnitude > 0.15f)
        {
            newDirection.Normalize();

            this.MovementDirection = newDirection;
            this.transform.xLookAt(this.transform.position + newDirection.xToVector3());

            //float angle = transform.up.xAngleSigned(MovementDirection, transform.forward);
            //float perFrame = Time.fixedDeltaTime * AngularSpeed;
            //float clampedAngle = Mathf.Clamp(angle, -perFrame, perFrame);
            //transform.Rotate(transform.forward, clampedAngle);

            this.Animator.SetBool("Moving", true);
        }
        else
        {
            this.MovementDirection = Vector2.zero;

            this.Animator.SetBool("Moving", false);
        }

        base.FixedUpdate();
    }
}
