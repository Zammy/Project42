﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Level : SingletonBehavior<Level>
{
    public Transform LevelBase;
    public Transform CreaturesBase;

    List<LevelObj> objects;
    List<GameObject> creatures;

    List<List<bool>> impassableMap;

    public bool IsPassable(Point p)
    {
        if (impassableMap[p.X] == null)
        {
            return true;
        }

        if (impassableMap[p.X].Count <= p.Y)
        {
            return true;
        }

        return !impassableMap[p.X][p.Y];
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

    public void RemoveObject(LevelObj o)
    {
        objects.Remove(o);

        AddImpassability(o.Collider, false);
    }

    public List<GameObject> GetObstaclesAround(Vector3 pos, float distance)
    {
        return GetObjectsAround(LevelObjType.Obastacle, pos, distance)
             .Select<LevelObj, GameObject>(o => o.gameObject).ToList();
    }

    public List<LevelObj> GetObjectsWithDangerAround(Vector3 pos, float distance)
    {
        return GetObjectsAround(LevelObjType.All, pos, distance);
    }

    protected override void Awake()
    {
        base.Awake();

        this.objects = new List<LevelObj>();
        this.creatures = new List<GameObject>();
        this.impassableMap = new List<List<bool>>();

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

    List<LevelObj> GetObjectsAround(LevelObjType type, Vector3 pos, float distance)
    {
        var foundList = new List<LevelObj>();

        float sqrDist = distance * distance;

        foreach (var lgo in objects)
        {
            if (lgo.Type == type || type == LevelObjType.All)
            {
                if ((lgo.transform.position - pos).sqrMagnitude < sqrDist)
                {
                    foundList.Add(lgo);
                }
            }
        }

        return foundList;
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

                AddImpassability(collider);
            }
        }
    }

    private void AddImpassability(BoxCollider2D collider, bool impassable = true)
    {
        var bounds = collider.bounds;
        for (int x = (int)(bounds.center.x - bounds.extents.x + 0.5f); x <= bounds.center.x + bounds.extents.x - 0.5f; x++)
        {
            for (int y = (int)(bounds.center.y - bounds.extents.y + 0.5f); y <= bounds.center.y + bounds.extents.y - 0.5f; y++)
            {
                SetImpassability(x, y, impassable);
            }
        }
    }

    void SetImpassability(int x, int y, bool impassable)
    {
        impassableMap.xAddUpTo(x);
        if (impassableMap[x] == null)
        {
            impassableMap[x] = new List<bool>(y);
        }
        impassableMap[x].xAddUpTo(y);
        impassableMap[x][y] = impassable;
    }
}
