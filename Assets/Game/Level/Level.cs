using UnityEngine;
using System.Collections.Generic;

public class Level : SingletonBehavior<Level>
{
    public Transform LevelBase;
    public Transform CreaturesBase;

    List<LevelObj> objects;
    List<GameObject> creatures;

    List<List<bool>> impassable;

    public bool IsPassable(Point p)
    {
        if (impassable[p.X] == null)
        {
            return true;
        }

        if (impassable[p.X].Count <= p.Y)
        {
            return true;
        }

        return !impassable[p.X][p.Y];
    }

    public bool IsPassable(Vector3 pos)
    {
        Point p = pos.xToPoint();
        return this.IsPassable(p);
    }

    public List<GameObject> GetCreaturesAround(Transform trans, float searchRange)
    {
        var creaturesAround = new List<GameObject>();
        float sqredRng = searchRange * searchRange;
        for (int i = 0; i < creatures.Count; i++)
        {
            var creature = creatures[i];
            if (creature != trans.gameObject &&
                 (creature.transform.position - trans.position).sqrMagnitude < sqredRng)
            {
                creaturesAround.Add(creature);
            }
        }

        return creaturesAround;
    }

    public void RemoveCreature(GameObject creature)
    {
        this.creatures.Remove(creature);
    }

    protected override void Awake()
    {
        base.Awake();

        this.objects = new List<LevelObj>();
        this.creatures = new List<GameObject>();
        this.impassable = new List<List<bool>>();

        if (CreaturesBase != null)
        {
            foreach (Transform creature in CreaturesBase)
            {
                creatures.Add(creature.gameObject);
            }
        }

        if (this.transform.childCount > 0)
        {
            this.GetObjectsFrom(this.transform);
        }
    }

    void GetObjectsFrom(Transform t)
    {
        foreach (Transform tileTrns in t)
        {
            var obj = tileTrns.gameObject.GetComponent<LevelObj>();
            var collider = obj.Collider;
            if (collider != null)
            {
                this.objects.Add(obj);

                var bounds = collider.bounds;
                for (int x = (int)(bounds.center.x - bounds.extents.x + 0.5f); x <= bounds.center.x + bounds.extents.x - 0.5f; x++)
                {
                    for (int y = (int)(bounds.center.y - bounds.extents.y + 0.5f); y <= bounds.center.y + bounds.extents.y - 0.5f; y++)
                    {
                        SetImpassable(x, y);
                    }
                }
            }
        }
    }

    void SetImpassable(int x, int y)
    {
        impassable.xAddUpTo(x);
        if (impassable[x] == null)
        {
            impassable[x] = new List<bool>(y);
        }
        impassable[x].xAddUpTo(y);
        impassable[x][y] = true;
    }
}
