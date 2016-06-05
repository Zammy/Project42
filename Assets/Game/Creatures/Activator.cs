using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Activator : AIState
{
    public bool ShouldBeVisible = false;
    public float Range = 10f;
    public float ActivateCreaturesAround = 0f;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);
        this.Priority = 0;
    }

    public override void OnExit(AIState nextState)
    {
        base.OnExit(nextState);
    }


    public override void StateUpdate()
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
                    Activate();
                }
            }
            else
            {
                Activate();
            }
        }
    }

    public void RemoteActivate()
    {
        if (this.StateManager.ActiveState == this)
        {
            this.Activate();
        }
    }

    void Activate()
    {
        this.StateManager.ActivateStateWithHighestPriorty();
        List<GameObject> creaturesAround = Level.Instance.GetCreaturesAround(this.creatureTransform, ActivateCreaturesAround);
        for (int i = 0; i < creaturesAround.Count; i++)
        {
            var creature = creaturesAround[i];
            var aiStateManager = creature.GetComponent<AIStateManager>();
            if (aiStateManager.ActiveState.GetType() == typeof(Activator) )
            {
                var activator = aiStateManager.GetState<Activator>();
                activator.RemoteActivate();
            }
        }
    }
}
