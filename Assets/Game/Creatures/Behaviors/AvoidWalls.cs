﻿using System.Collections.Generic;
using UnityEngine;

public class AvoidWalls : AIBehavior
{
    public int Range = 2;

    Level level;

    void Awake()
    {
        level = Level.Instance;
    }

    //List<Tile> previousWallsAround = null;

    public override Vector2[] GetDanger()
    {
        //if (previousWallsAround != null)
        //{
        //    foreach (var item in previousWallsAround)
        //    {
        //        item.DebugHighlight = false;
        //    }
        //}

        List<Tile> wallsAround = level.GetImpassableAround(CreatureTransform.position, Range);
        var danger = new List<Vector2>();
        for (int i = 0; i < wallsAround.Count; i++)
        {
            Vector3 diff = CreatureTransform.position - wallsAround[i].transform.position;
            float magnitude = diff.magnitude;
            if (Range < magnitude)
                continue;

            //wallsAround[i].DebugHighlight = true;

            float finalMagnitude = Mathf.Pow(Range - diff.magnitude, 4);

            diff = diff.normalized * finalMagnitude * this.Strength;
            danger.Add(diff);
        }

        //this.previousWallsAround = wallsAround;

        return danger.ToArray();
    }

    public override Vector2[] GetInterest()
    {
        return AIBehavior.Empty;
    }
}