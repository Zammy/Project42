using System;
using UnityEngine;

public abstract class AIMovingState : AIState
{
    const float FRAME_MIN_DISTANCE = 0.01f;
    const int FRAME_SAMPLE_RATE = 15;
    int frameCounter = 0;
    Vector2 samplePos;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        samplePos = creatureTransform.position;
    }

    public override void StateUpdate(AIStateManager mng)
    {
        if (++frameCounter % FRAME_SAMPLE_RATE == 0)
        {
            Vector2 currentPos = creatureTransform.position.xToVector2();
            float sqrDist = (currentPos - samplePos).sqrMagnitude;
            if (sqrDist < FRAME_MIN_DISTANCE * FRAME_SAMPLE_RATE)
            {
                this.OnNotMoving(mng);
            }
            this.samplePos = currentPos;
        }
    }

    protected abstract void OnNotMoving(AIStateManager mng);
}