using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    public AIBehavior[] Behaviors;

    public int Priority;

    public virtual void OnEnter(AIState previousState)
    {
        Debug.LogFormat("OnEnter {0}", GetType().Name);
        foreach (var bhv in Behaviors)
        {
            bhv.enabled = true;
        }
    }
    public virtual void OnExit(AIState nextState)
    {
        Debug.LogFormat("OnExit {0}", GetType().Name);
        foreach (var bhv in Behaviors)
        {
            bhv.enabled = false;
        }
    }

    public abstract void StateUpdate(AIStateManager mng);

    protected Transform creatureTransform
    {
        get
        {
            return this.transform.parent.transform;
        }
    }
}