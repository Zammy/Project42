using UnityEngine;
using System.Collections;
using System;

public class Wonder : AIBehavior
{
    public float circleForward = 1f;
    public float circleSize = 1f;
    public float angleChangerPerSec = 15f;
        
    Vector3 displacement;

    void Start()
    {
        displacement = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    public override Vector3[] GetDanger()
    {
        return AIBehavior.Empty;
    }

    public override Vector3[] GetInterest()
    {

        displacement = Quaternion.Euler(0, angleChangerPerSec * Time.deltaTime, 0) * displacement;

        Vector3 circleCenter = transform.forward * circleForward;

        Debug.DrawRay(this.transform.position + circleCenter, displacement, Color.red);

        Vector3 final = circleCenter + (displacement * circleSize) * Strength;

        //Debug.DrawRay(this.transform.position, final, Color.green);

        //return AIBehavior.Empty;

        return new Vector3[]
        {
            final
        };
    }
}
