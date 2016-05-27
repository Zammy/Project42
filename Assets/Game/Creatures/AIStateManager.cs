﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIStateManager : MonoBehaviour
{
    public Transform StatesBase;

    public string CurrentState;

    AIState[] allStates;
    int activeStateIndex;

    void Awake()
    {
        this.allStates = StatesBase.GetComponents<AIState>();
    }

    void Start()
    {
        ActivateStateWithHighestPriorty();
    }

    void Update()
    {
        for (int i = 0; i < this.allStates.Length; i++)
        {
            var globalState = allStates[i] as AIGlobalState;
            if (globalState != null
                && globalState.ShouldActivate()
                && allStates[activeStateIndex] != globalState)
            {
                this.ActivateState(i);
            }
        }

        allStates[activeStateIndex].StateUpdate(this);
    }

    public void ActivateState<T>() where T : AIState
    {
        int newStateIndex = -1;
        for (int i = 0; i < allStates.Length; i++)
        {
            if (allStates[i].GetType() == typeof(T))
            {
                newStateIndex = i;
            }
        }

        if (newStateIndex == -1)
        {
            throw new UnityException("Could not find state of type " + typeof(T));
        }

        this.ActivateState(newStateIndex);
    }

    public void ActivateStateWithHighestPriorty()
    {
        int highestPriority = int.MinValue;
        int startingState = -1;
        for (int i = 0; i < allStates.Length; i++)
        {
            int priority = allStates[i].Priority;
            if (highestPriority < priority)
            {
                highestPriority = priority;
                startingState = i;
            }
        }

        if (startingState == -1)
        {
            throw new UnityException("Could not decide on starting state");
        }

        this.activeStateIndex = startingState;
        allStates[activeStateIndex].OnEnter(null);
    }

    void ActivateState(int newStateIndex)
    {
        allStates[activeStateIndex].OnExit(allStates[newStateIndex]);
        int prevStateIndex = activeStateIndex;
        activeStateIndex = newStateIndex;
        allStates[activeStateIndex].OnEnter(allStates[prevStateIndex]);

        this.CurrentState = allStates[activeStateIndex].GetType().Name;
    }

}