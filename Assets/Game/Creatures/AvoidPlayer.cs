using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class AvoidPlayer : AIMovingState
{
    public float Range;

    Vector3 diffFromCrew;
    Avoid avoid;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);
        avoid = this.Behaviors.OfType<Avoid>().First();
    }

    public override void OnExit(AIState nextState)
    {
        base.OnExit(nextState);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        var playerPos = this.PlayerPos;
        avoid.Goal = playerPos;

        if ((playerPos - this.CreatureTransform.position).magnitude > Range)
        {
            this.OnCrewAvoided();
        }
    }

    protected override void OnNotMoving()
    {
        this.OnCrewAvoided();
    }

    protected abstract void OnCrewAvoided();
}
