using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class AvoidCrew : AIMovingState
{
    public float Range;

    Vector2 diffFromCrew;
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

        Vector2 crewPos = Crew.Instance.transform.position.xToVector2();
        avoid.Goal = crewPos;

        if ((crewPos - this.creatureTransform.position.xToVector2()).magnitude > Range)
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
