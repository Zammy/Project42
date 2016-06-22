using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    public GameObject CreatureObject;

    public AIBehavior[] Behaviors;
    public int Priority;

    CharactersManager charMnger;

    void Awake()
    {
        charMnger = this.CreatureTransform.parent.GetComponent<CharactersManager>();
    }

    protected AIStateManager StateManager
    {
        get
        {
            return CreatureObject.GetComponent<AIStateManager>();
        }
    }

    protected Transform CreatureTransform
    {
        get
        {
            return CreatureObject.transform;
        }
    }

    protected Vector3 PlayerPos
    {
        get
        {
            return charMnger.Player.transform.position;
        }
    }

    public virtual void OnEnter(AIState previousState)
    {
//        Debug.LogFormat("OnEnter {0}", GetType().Name);
        foreach (var bhv in Behaviors)
        {
            bhv.enabled = true;
        }
    }
    public virtual void OnExit(AIState nextState)
    {
//        Debug.LogFormat("OnExit {0}", GetType().Name);
        foreach (var bhv in Behaviors)
        {
            bhv.enabled = false;
        }
    }

    public abstract void StateUpdate();

}