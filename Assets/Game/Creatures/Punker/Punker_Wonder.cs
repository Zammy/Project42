using UnityEngine;
using System.Collections;

public class Punker_Wonder : AIMovingState 
{
    public float Range = 2f;

    Seek seek;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        seek = (Seek) this.Behaviors[0];

        var diff = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Range;

        seek.Goal = CreatureTransform.position + diff;
    }

    protected override void OnNotMoving()
    {
        StateManager.ActivateState<Punker_StupifiedState>();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (seek.Goal == Vector3.zero)
        {
            StateManager.ActivateState<Punker_StupifiedState>();
        }
    }
}
