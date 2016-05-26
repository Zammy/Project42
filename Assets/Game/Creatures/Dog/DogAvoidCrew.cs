using UnityEngine;
using System.Collections;
using System;

public class DogAvoidCrew : AvoidCrew
{
    protected override void OnCrewAvoided(AIStateManager mng)
    {
        mng.ActivateState<DogBark>();
    }
}
