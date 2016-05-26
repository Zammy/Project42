using UnityEngine;
using System.Collections;
using System;

public class Avoid : AIBehavior 
{
    public Vector3 Goal = Vector3.zero;

    void Start() { }

    public override Vector2[] GetDanger()
    {
        //var goalDirection = this.CalculateDirection();
        //if (goalDirection == Vector2.zero)
        //{
        //    return AIBehavior.Empty;
        //}

        //var interestContext = new Vector2[5];
        //interestContext[0] = goalDirection;
        //interestContext[1] = Quaternion.Euler(0, 0, -45f) * interestContext[0];
        //interestContext[2] = Quaternion.Euler(0, 0, 45f) * interestContext[0];
        //interestContext[3] = Quaternion.Euler(0, 0, -90f) * interestContext[0];
        //interestContext[4] = Quaternion.Euler(0, 0, 90f) * interestContext[0];

        //interestContext[1] *= 0.9f;
        //interestContext[2] *= 0.9f;
        //interestContext[3] *= 0.7f;
        //interestContext[4] *= 0.7f;

        //return interestContext;
        return AIBehavior.Empty;
    }

    public override Vector2[] GetInterest()
    {
        var goalDirection = this.CalculateDirection();
        if (goalDirection == Vector2.zero)
        {
            return AIBehavior.Empty;
        }

        var interestContext = new Vector2[5];
        interestContext[0] = goalDirection;
        interestContext[1] = Quaternion.Euler(0, 0, -45f) * interestContext[0];
        interestContext[2] = Quaternion.Euler(0, 0, 45f) * interestContext[0];
        interestContext[3] = Quaternion.Euler(0, 0, -90f) * interestContext[0];
        interestContext[4] = Quaternion.Euler(0, 0, 90f) * interestContext[0];

        interestContext[1] *= 0.9f;
        interestContext[2] *= 0.9f;
        interestContext[3] *= 0.7f;
        interestContext[4] *= 0.7f;

        return interestContext;
    }

    private Vector2 CalculateDirection()
    {
        if (Goal == Vector3.zero)
        {
            return Vector2.zero;
        }

        Vector3 diff = CreatureTransform.position - this.Goal;
        if (diff.sqrMagnitude < 0.05f)
        {
            Goal = Vector3.zero;
            return Vector2.zero;
        }
        return diff.normalized * this.Strength;
    }
}
