using UnityEngine;
using System.Collections;
using System;

public class DogSeekCrew : SeekCrew
{
    public float AngleVariance;

    protected override Vector2 RandomDiffFromCrew()
    {
        Vector2 diff = base.RandomDiffFromCrew();
        do
        {
            diff = Quaternion.AngleAxis(UnityEngine.Random.Range(-AngleVariance, AngleVariance), Vector3.forward) * diff;
        }
        while (!Level.Instance.IsPassable(Crew.Instance.transform.position.xToVector2() + diff));

        return diff;
    }

    protected override void OnCrewSought()
    {
        StateManager.ActivateState<DogAttack>();
    }

    protected override void OnNotMoving()
    {
        StateManager.ActivateState<DogBark>();
    }
}
