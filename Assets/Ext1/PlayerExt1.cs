using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;


public class PlayerExt1 : MonoBehaviour 
{
    private class POIComparer : IComparer<POI>
    {   
        Vector3 playerPos;

        public POIComparer(Vector3 playerPos)
        {
            this.playerPos = playerPos;
        }

        #region IComparer implementation
        public int Compare(POI x, POI y)
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

    private enum PointType
    {
        Facing,
        Passing
    }
    public LevelExt1 Level;
    public Transform LOS;

    public GameObject DebugPointPrefab;

    List<POI> pois;

    const float GOES_THROUGH = 0.7071f;
    const float IGNORE = -0.7071f;


    void Start()
    {
        var meshFilter = this.LOS.GetComponent<MeshFilter>();
        var m = new Mesh();
        meshFilter.mesh = m;

//        this.GenerateLOSMesh();
    }


    void Update()
    {
        GenerateLOSMesh();
    }

    bool drawHelpPoints = true;
    Vector3 lastPos = Vector3.one;
    Dictionary<int, GameObject> helpPoints = new Dictionary<int, GameObject>();

    void DrawHelpPointAt(int i, Vector3 at, Color color)
    {
        GameObject go;
        if (helpPoints.ContainsKey(i))
        {
            go = helpPoints[i];
        }
        else
        {
            go = (GameObject)Instantiate(DebugPointPrefab);
            helpPoints.Add(i, go);
        }
        go.GetComponent<SpriteRenderer>().color = color;
        go.transform.position = at;
    }

	// Use this for initialization
	void GenerateLOSMesh ()
    {
        if ((this.transform.position - this.lastPos).sqrMagnitude > 1f)
        {
            drawHelpPoints = true;
            lastPos = transform.position;
        }
        this.pois = this.Level.GetPOIS();
        var playerPos = this.transform.position;

        pois.Sort(new POIComparer(playerPos));

        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(playerPos);
        List<int> triangles = new List<int>();


        System.Action<Vector3> addVertex = (Vector3 vertex) =>
        {
            Debug.LogFormat("Add Vertex {0}", vertex);
            
            vertices.Add(vertex);
        };


        PointType firstPointType;
        PointType lastPointType = (PointType) 0; //just to stop the compiler complaining
//        float lastPointDistance;

        for (int i = 0; i < pois.Count; i++)
        {
            POI poi = pois[i];

            Vector3 fromPOItoPos = playerPos - poi.Pos;
            float dot = Vector3.Dot(fromPOItoPos.normalized, poi.GetNormal());
            if (dot > GOES_THROUGH)
            {
                if (drawHelpPoints)
                {
                    DrawHelpPointAt(i, poi.Pos, Color.green);
                }

//                Debug.DrawRay(playerPos, -fromPOItoPos, Color.green, fromPOItoPos.magnitude);

                RaycastHit2D hit = Physics2D.Raycast(playerPos, -fromPOItoPos, Camera.main.orthographicSize * 2);
                if ( ( new Vector3(hit.point.x, hit.point.y) - poi.Pos).sqrMagnitude > 1f )
                {
                    continue;
                }

                addVertex(poi.Pos);

                Debug.LogFormat("[{0}] Facing", vertices.Count -1);

                if (i > 0)
                {
                    AddTriangle(PointType.Facing, lastPointType, vertices, triangles);
                }
                else
                {
                    firstPointType = PointType.Facing;
                }

                lastPointType = PointType.Facing;
//                lastPointDistance = fromPOItoPos.sqrMagnitude;
  
            }
            else if ( dot > IGNORE && dot < GOES_THROUGH )
            {
                if (drawHelpPoints)
                {
                    DrawHelpPointAt(i, poi.Pos, Color.yellow);
                }

//                Debug.DrawRay(playerPos, -fromPOItoPos, Color.yellow, fromPOItoPos.magnitude);

                RaycastHit2D[] hits = Physics2D.RaycastAll(playerPos, -fromPOItoPos, Camera.main.orthographicSize * 2);
                Vector3 hit0Pos = new Vector3(hits[0].point.x, hits[0].point.y);
                if ( (hit0Pos - poi.Pos).sqrMagnitude < 1f ) //hits POI point
                {
                    addVertex(poi.Pos);
                    addVertex(hits[1].point);

                    Debug.Log("#Hits POI " + i.ToString());
                }
                else if ((hit0Pos- playerPos).sqrMagnitude > fromPOItoPos.sqrMagnitude) //hits behind poi point
                {
                    addVertex(poi.Pos);
                    addVertex(hits[0].point);

                    Debug.Log("#Hits behind POI " + i.ToString());
                }
                else //hits point before that so ignore it
                {
                    Debug.Log("#Hits BEFORE POI " + i.ToString());

                    continue;
                }

                Debug.LogFormat("[{0} - {1}] Passing", vertices.Count -2, vertices.Count -1);

                if (i > 0)
                {
                    AddTriangle(PointType.Passing, lastPointType, vertices, triangles);
                }
                else
                {
                    firstPointType = PointType.Passing;
                }

                lastPointType = PointType.Passing;
            }
            else
            {
                if (drawHelpPoints)
                {
                    DrawHelpPointAt(i, poi.Pos, Color.red);
                }
                continue;
            }

//            if (i > 0)
//            {
//                addTriangle( vertices.Count-1, vertices.Count-2);
//            }
        }

//        AddTriangle(firstPointType, lastPointType, )

        triangles.Add(0);
        triangles.Add(vertices.Count-1);
        triangles.Add(1);
            

        var meshFilter = this.LOS.GetComponent<MeshFilter>();
        var m = meshFilter.mesh;
        m.Clear();
        m.vertices = vertices.ToArray();
//        m.uv = new Vector2[] { new Vector2 (0, 0), new Vector2 (0, 1), new Vector2(1, 1), new Vector2 (1, 0) };
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();
        meshFilter.mesh = m;

//        textStyle.normal.textColor = Color.cyan;


        drawHelpPoints = false;
	} 

    void AddTriangle( PointType currentPointType, PointType lastPointType, List<Vector3> vertices, List<int> triangles)
    {
        System.Action<int, int> addTriangle = ((int i1, int i2) => 
        {
            triangles.Add(0);
            if (i1 > i2)
            {
                Debug.LogFormat("Add triangle {0} -> {1} -> {2}",0, i1, i2);

                triangles.Add(i1);
                triangles.Add(i2);
            }
            else
            {
                Debug.LogFormat("Add triangle {0} -> {2} -> {1}",0, i1, i2);

                triangles.Add(i2);
                triangles.Add(i1);
            }
        });

        Vector3 playerPos = this.transform.position;

        if (currentPointType == PointType.Facing && lastPointType == PointType.Facing)
        {
            Debug.Log("Adding Facing Facing");
            addTriangle(vertices.Count - 1, vertices.Count - 2);
            return;
        }

        int facingPointIndex;
        int passingPointIndex;
        int passingPointIndex2;

        if (currentPointType == PointType.Facing && lastPointType == PointType.Passing)
        {
            Debug.Log("Adding Facing Passing");

            facingPointIndex = vertices.Count -1;
            passingPointIndex2 = vertices.Count - 2;
            passingPointIndex = vertices.Count - 3;
        }
        else if (currentPointType == PointType.Passing && lastPointType == PointType.Facing)
        {
            Debug.Log("Adding Passing Facing");

            passingPointIndex2 = vertices.Count - 1;
            passingPointIndex = vertices.Count - 2;
            facingPointIndex = vertices.Count - 3;
        }
        else
        {
            throw new UnityException("This should not happen");
        }
             
        Vector3 facingPoint  = vertices[facingPointIndex];
        Vector3 passingPoint = vertices[passingPointIndex];
        Vector3 passingPoint2 = vertices[passingPointIndex2];

        float facingPointDist = (playerPos - facingPoint).sqrMagnitude;
        float passingPointDist = (playerPos - passingPoint).sqrMagnitude;

        if (facingPointDist > passingPointDist)
        {
            addTriangle(facingPointIndex, passingPointIndex2);
        }
        else
        {
            addTriangle(facingPointIndex, passingPointIndex);
        }
    }
    
//    void OnGUI()
//    {
//        Vector3 pos = this.transform.position;
//        for (int i = 0; i < 4; i++)
//        {
//            Vector3 pointPos = points[i,0];
//            Vector3 normal = points[i,1];
//
//            Vector3 diff = pos - pointPos;
//
//            float dot = Vector3.Dot(diff.normalized, normal.normalized);
//
//            Handles.Label(pointPos + normal, dot.ToString());
//        }
//    }

//    GUIStyle textStyle = new GUIStyle();
//    
    void OnGUI()
    {
        if (pois == null) return;

        Vector3 pos = this.transform.position;
        for (int i = 0; i < pois.Count; i++)
        {
            Vector3 p = pois[i].Pos;
            Handles.Label(p, i.ToString());
//            Handles.Label(new Vector3(p.x, p.y-.5f), pois[i].AngleTo(pos).ToString(), textStyle );
        }
    }

}
