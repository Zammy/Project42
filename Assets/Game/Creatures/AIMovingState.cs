using System;
using UnityEngine;

public abstract class AIMovingState : AIState
{
    public int FramesIdle = 30;

    const float FRAME_MIN_DISTANCE = 0.01f;
    int frameCounter = 0;
    Vector2 samplePos;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        samplePos = CreatureTransform.position;
    }

    public override void StateUpdate()
    {
        if (++frameCounter % FramesIdle == 0)
        {
            Vector2 currentPos = CreatureTransform.position.xToVector2();
            float sqrDist = (currentPos - samplePos).sqrMagnitude;
            if (sqrDist < FRAME_MIN_DISTANCE * FramesIdle)
            {
                this.OnNotMoving();
            }
            this.samplePos = currentPos;
        }
    }

    protected abstract void OnNotMoving();
}