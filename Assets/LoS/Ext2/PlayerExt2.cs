using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;



public class PlayerExt2 : MonoBehaviour 
{
    private class EndPointComparer : IComparer<EndPoint>
    {   
        Vector3 playerPos;

        public EndPointComparer(Vector3 playerPos)
        {
            this.playerPos = playerPos;
        }

        #region IComparer implementation
        public int Compare(EndPoint x, EndPoint y)
        {
            if (x.AngleTo(playerPos) > y.AngleTo(playerPos))
            {
                return 1;
            }
            else if (x.AngleTo(playerPos) == y.AngleTo(playerPos))
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        #endregion
    }

    public LevelExt2 Level;
//    public GameObject DebugPointPrefab;

    List<EndPoint> endPoints = new List<EndPoint>();


    void Start()
    {
        var playerPos = new Vector2(this.transform.position.x, this.transform.position.y);
        var segments = this.Level.GetSegments();

        for (int i = segments.Count - 1; i >= 0; i--)
        {
            var seg = segments[i];
            var diff = seg.Points[0] - playerPos;
            diff.Normalize();
            float dot = Vector2.Dot(seg.GetNormal(), diff);

            if (dot > 0)
            {
                segments.RemoveAt(i);
            }
        }

        foreach (var seg in segments)
        {
            endPoints.AddRange(seg.GetEndPoints());
        }

        endPoints.Sort(new EndPointComparer(this.transform.position));


    }


    void OnGUI()
    {
        if (endPoints == null) return;

        Vector3 pos = this.transform.position;
        for (int i = 0; i < endPoints.Count; i++)
        {
            Vector3 p = endPoints[i].GetPoint();
            Handles.Label(p, i.ToString());
//            Handles.Label(new Vector3(p.x, p.y-.5f), pois[i].AngleTo(pos).ToString(), textStyle );
        }
    }

//
//    bool drawHelpPoints = true;
//    Vector3 lastPos = Vector3.one;
//    Dictionary<int, GameObject> helpPoints = new Dictionary<int, GameObject>();
//
//    void DrawHelpPointAt(int i, Vector3 at, Color color)
//    {
//        GameObject go;
//        if (helpPoints.ContainsKey(i))
//        {
//            go = helpPoints[i];
//        }
//        else
//        {
//            go = (GameObject)Instantiate(DebugPointPrefab);
//            helpPoints.Add(i, go);
//        }
//        go.GetComponent<SpriteRenderer>().color = color;
//        go.transform.position = at;
//    }
}
