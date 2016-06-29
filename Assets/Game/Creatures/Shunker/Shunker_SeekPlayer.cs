using UnityEngine;
using System.Collections;

public class Shunker_SeekPlayer : AIMovingState 
{
    public MoveMode Charge;

    public float HowClose;

    Seek seek;
    CreatureMovement movement;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        seek = this.Behaviors[0] as Seek;

        movement = CreatureObject.GetComponent<CreatureMovement>();

        movement.SetMovementMode(Charge);
    }

    public override void OnExit(AIState nextState)
    {
        base.OnExit(nextState);

        movement.ResetMovementMode();
    }

    protected override void OnNotMoving()
    {
        StateManager.ActivateState<Shunker_StupifiedState>();
    }

    public override void StateUpdate()
    {
        seek.Goal = PlayerPos;

        float target = HowClose;
        float sqrDist = (CreatureTransform.position - PlayerPos).magnitude;

        if (target > sqrDist)
        {
            StateManager.ActivateState<Shunker_AttackPlayer>();
        }
    }
}
