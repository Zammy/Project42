using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Punker_StupifiedState : AIState 
{
    public float WaitForLeast = 0.15f;
    public float WaitForMost = 0.30f;


    float waitUntil = float.MaxValue;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        waitUntil = Time.time + Random.Range(WaitForLeast, WaitForMost);

        this.transform.DOLookAt( PlayerPos - CreatureTransform.position, 0.5f );
    }

	public override void StateUpdate()
    {
        if (Time.time > waitUntil)
        {
            this.StateManager.ActivateState<Punker_SeekPlayer>();
        }
    }
}
