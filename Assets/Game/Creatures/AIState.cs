using UnityEngine;
using System.Collections;

public abstract class AIState : MonoBehaviour
{
    public AIBehavior[] Behaviors;

    public abstract void OnEnter();
    public abstract void OnExit();
}
