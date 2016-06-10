using System.Collections.Generic;
using UnityEngine;

public class AvoidWalls : AIBehavior
{
    public float Range = 2;

    List<Vector2> dangerVectors = new List<Vector2>();

    void Start() { }

    void Update()
    {
        foreach (var vec in dangerVectors)
        {
            Debug.DrawRay(CreatureTransform.position, vec.xToVector3(), Color.red, vec.magnitude);
        }
    }

    public override Vector2[] GetDanger()
    {
        Vector3 selfPos = this.CreatureTransform.position;
        Vector2 selfPos2d = selfPos.xToVector2();

        dangerVectors.Clear();

        List<LevelObj> wallsAround = Level.Instance.GetObjectsWithDangerAround(selfPos, Range);
        for (int i = 0; i < wallsAround.Count; i++)
        {
            //SteerDanger steerDanger = wallsAround[i].transform.position
            Vector3 diff = selfPos - wallsAround[i].transform.position;
            float distance = diff.magnitude;

            Vector2 dangerVec = diff * Mathf.Pow(Range - distance, 4) * Strength;

            dangerVectors.Add(dangerVec);
        }

        return dangerVectors.ToArray();
    }

    public override Vector2[] GetInterest()
    {
        return AIBehavior.Empty;
    }
}