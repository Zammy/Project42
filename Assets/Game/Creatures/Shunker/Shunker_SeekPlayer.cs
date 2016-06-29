using UnityEngine;
using System.Collections;

public class Shunker_SeekPlayer : AIState //AIMovingState 
{
    public float HowClose;

    Seek seek;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        seek = this.Behaviors[0] as Seek;
    }

//    protected override void OnNotMoving()
//    {
//        StateManager.ActivateState<Punker_StupifiedState>();
//    }

    public override void StateUpdate()
    {
//        base.StateUpdate();

        seek.Goal = PlayerPos;

        float against = HowClose * HowClose;
        float sqrDist = (CreatureTransform.position - PlayerPos).sqrMagnitude;

        if (against > sqrDist)
        {
            StateManager.ActivateState<Shunker_AttackPlayer>();
        }
    }
}
