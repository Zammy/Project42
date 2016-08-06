using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class RandomPosAround : Action 
{
    //Set through Unity
    public float Distance;
    public Transform Around;
    public SharedVector3 Result;
    //


    public override TaskStatus OnUpdate()
    {
        var direction = (new Vector3( Random.Range(-1f, 1f), 0,  Random.Range(-1f, 1f))).normalized;
        Result.Value = Around.position + direction * Distance;
        return TaskStatus.Success;
    }
}
