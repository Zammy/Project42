using UnityEngine;
using System.Collections;

public class Seek : AIBehavior 
{
    //Set through Unity
    public float Weight;
    //

    public Vector3 Goal = Vector3.zero;

    public override Vector2 CalculateDirection()
    {
        if (Goal == Vector3.zero)
        {
            return Vector2.zero;
        }

        Vector3 diff = this.Goal - this.transform.position ;
        if (diff.sqrMagnitude < 0.05f)
        {
            Goal = Vector3.zero;
            return Vector2.zero;
        }
        return diff.normalized * Weight;
    }
}
