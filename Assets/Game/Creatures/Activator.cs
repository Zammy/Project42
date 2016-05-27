using System;
using System.Collections;
using UnityEngine;

public class Activator : AIState
{
    public bool ShouldBeVisible = false;
    public float Range = 10f;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);
        this.Priority = 0;
    }

    public override void OnExit(AIState nextState)
    {
        base.OnExit(nextState);
    }

    public override void StateUpdate(AIStateManager mng)
    {
        Vector3 fromCreatureToCrew = Crew.Instance.transform.position - creatureTransform.position;
        float distance = fromCreatureToCrew.magnitude;
        if (distance < Range)
        {
            if (ShouldBeVisible)
            {
                RaycastHit2D hit = Physics2D.Raycast(creatureTransform.position, fromCreatureToCrew);
                if (hit.collider != null && hit.collider.tag == "Crew")
                {
                    mng.ActivateStateWithHighestPriorty();
                }
            }
            else
            {
                mng.ActivateStateWithHighestPriorty();
            }
        }
    }
}
