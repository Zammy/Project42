using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AvoidCreatures : AIBehavior
{
    public int Range = 2;

    Level level;
    List<Vector2> dangers = new List<Vector2>();

    // Use this for initialization
    void Awake()
    {
        level = Level.Instance;
    }

    public override Vector2[] GetDanger()
    {
        dangers.Clear();
        List<GameObject> creaturesAround = level.GetCreaturesAround(CreatureTransform.position, Range);
        for (int i = 0; i < creaturesAround.Count; i++)
        {
            var creature = creaturesAround[i];

            var diff = CreatureTransform.position - creature.transform.position;
            float finalMagnitude = Mathf.Pow(Range - diff.magnitude, 1.5f);
            diff = diff.normalized * finalMagnitude * this.Strength;

            dangers.Add(diff);

        }
        return dangers.ToArray();
    }

    public override Vector2[] GetInterest()
    {
        return AIBehavior.Empty;
    }

}
