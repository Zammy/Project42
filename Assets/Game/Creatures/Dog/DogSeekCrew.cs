using UnityEngine;
using System.Collections;
using System;

public class DogSeekCrew : SeekCrew
{
    public float AngleVariance;

    protected override Vector2 CalcDiffFromCrew()
    {
        Vector2 diff = base.CalcDiffFromCrew();
        return Quaternion.AngleAxis(UnityEngine.Random.Range(-AngleVariance, AngleVariance), Vector3.forward) * diff;
    }

    protected override void OnCrewSought(AIStateManager mng)
    {
        mng.ActivateState<DogAttack>();
    }
}
