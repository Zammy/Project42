using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class SeekCrew : AIState
{
    public float Range;

    Vector2 diffFromCrew;
    Seek seek;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        this.diffFromCrew = CalcDiffFromCrew();
        seek = this.Behaviors.OfType<Seek>().First();
    }

    public override void OnExit(AIState nextState)
    {
        base.OnExit(nextState);
    }

    public override void StateUpdate(AIStateManager mng)
    {
        Vector2 crewPos = Crew.Instance.transform.position.xToVector2();
        Vector2 goal = diffFromCrew + crewPos;

        seek.Goal = goal;
        float sqrDisToGoal = (creatureTransform.transform.position.xToVector2() - goal).sqrMagnitude;
        if (sqrDisToGoal < 0.5f)
        {
            this.OnCrewSought(mng);
        }
    }

    protected virtual Vector2 CalcDiffFromCrew()
    {
        Vector2 crewPos = Crew.Instance.transform.position.xToVector2();
        var diffFromCrew = (creatureTransform.position.xToVector2() - crewPos).normalized * Range;
        return diffFromCrew;
    }

    protected abstract void OnCrewSought(AIStateManager mng);
}
