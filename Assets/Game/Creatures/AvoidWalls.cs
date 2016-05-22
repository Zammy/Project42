using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AvoidWalls : AIBehavior
{
    public Level Level;
    public int Range = 5;

    void Start() { }

    List<Tile> previousWallsAround = null;

    public override Vector2[] GetDanger()
    {
        if (previousWallsAround != null)
        {
            foreach (var item in previousWallsAround)
            {
                item.DebugHighlight = false;
            }
        }

        List<Tile> wallsAround = Level.GetImpassableAround(this.transform.position, Range);
        var danger = new List<Vector2>();
        for (int i = 0; i < wallsAround.Count; i++)
        {
            Vector3 diff = this.transform.position - wallsAround[i].transform.position;
            float magnitude = diff.magnitude;
            if (Range < magnitude)
                continue;

            wallsAround[i].DebugHighlight = true;

            float finalMagnitude = Mathf.Pow(Range - diff.magnitude, 4);

            diff = diff.normalized * finalMagnitude * this.Strength;
            danger.Add(diff);
        }

        this.previousWallsAround = wallsAround;

        return danger.ToArray();
    }

    public override Vector2[] GetInterest()
    {
        return AIBehavior.Empty;
    }
}