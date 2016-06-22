using UnityEngine;
using System.Collections;
using System;

public class Seek : AIBehavior 
{
    public Vector3 Goal = Vector3.zero;

    Vector3[] interestContext = new Vector3[1];

    void Start() { }

    //void Update()
    //{
    //    foreach (var vec in interestContext)
    //    {
    //        Debug.DrawRay(CreatureTransform.position, vec.xToVector3(), Color.green, vec.magnitude);
    //    }
    //}

    public override Vector3[] GetDanger()
    {
        return AIBehavior.Empty;
    }

    public override Vector3[] GetInterest()
    {
        var goalDirection = this.CalculateDirection();
        if (goalDirection == Vector3.zero)
        {
            return AIBehavior.Empty;
        }

        interestContext[0] = goalDirection;
//        interestContext[1] = Quaternion.Euler(0, 0, -45f) * interestContext[0] ;
//        interestContext[2] = Quaternion.Euler(0, 0, 45f) * interestContext[0];
//        interestContext[3] = Quaternion.Euler(0, 0, -90f) * interestContext[0];
//        interestContext[4] = Quaternion.Euler(0, 0, 90f) * interestContext[0];
//
//        interestContext[1] *= 0.9f;
//        interestContext[2] *= 0.9f;
//        interestContext[3] *= 0.7f;
//        interestContext[4] *= 0.7f;

        return interestContext;
    }

    private Vector3 CalculateDirection()
    {
        if (Goal == Vector3.zero)
        {
            return Vector3.zero;
        }

        Vector3 diff = this.Goal - CreatureTransform.position;
        if (diff.sqrMagnitude < 0.05f)
        {
            Goal = Vector3.zero;
            return Vector3.zero;
        }
        return diff.normalized * this.Strength;
    }
}
