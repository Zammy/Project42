using System;
using System.Collections.Generic;
using UnityEngine;

public class Cohesion : AIBehavior
{
    public CreatureMovement[] OtherCreatures;

    void Start() { }

    public override Vector2[] GetDanger()
    {
        List<Vector2> dangers = new List<Vector2>();

        for (int i = 0; i < OtherCreatures.Length; i++)
        {
            var danger = OtherCreatures[i].transform.position - creatureTransform.position;
            danger *= this.Strength;
            dangers.Add(danger);
        }

        return dangers.ToArray();
    }

    public override Vector2[] GetInterest()
    {
        return AIBehavior.Empty;
    }
}

