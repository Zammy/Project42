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
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    const float GOES_THROUGH = 0.7071f;
    const float IGNORE = -0.7071f;


    void Start()
    {
        var meshFilter = this.LOS.GetComponent<MeshFilter>();
        var m = new Mesh();
        meshFilter.mesh = m;
    }

    void Update()
    {
        GenerateLOSMesh();
    }

    void DrawHelpPointAt(int i, Vector3 at, Color color)
    {
        GameObject go;
        go = (GameObject)Instantiate(DebugPointPrefab);
        go.GetComponent<SpriteRenderer>().color = color;
        go.transform.position = at;
    }

	// Use this for initialization
	void GenerateLOSMesh ()
    {
        float rayLength = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        Quaternion bitBack = Quaternion.AngleAxis(-0.1f, Vector3.forward);
        Quaternion bitForward = Quaternion.AngleAxis(0.1f, Vector3.forward);

        this.pois = this.Level.GetPOIS();

        Vector3 playerPos = this.transform.position;
        vertices.Clear();
        vertices.Add(playerPos);

        pois.Sort(new POIComparer(playerPos));

        for (int i = 0; i < pois.Count; i++)
        {
            POI poi = pois[i];

            Vector3 fromPOItoPos = playerPos - poi.Pos;
            float dot = Vector3.Dot(fromPOItoPos.normalized, poi.GetNormal());
            if (dot > GOES_THROUGH)
            {
                RaycastHit2D hit = Physics2D.Raycast(playerPos, -fromPOItoPos, rayLength);
                if (hit != null)
                {
                    vertices.Add(new Vector3(hit.point.x, hit.point.y));
                }
            }
            else if ( dot > IGNORE && dot < GOES_THROUGH )
            {
                Vector3 posToPoi = -fromPOItoPos;

                Vector3 bitBeforeRotated = bitBack * posToPoi;

                RaycastHit2D hit = Physics2D.Raycast(playerPos, bitBeforeRotated, rayLength);
                if (hit != null)
                {
                    vertices.Add(new Vector3(hit.point.x, hit.point.y));

                }

                hit = Physics2D.Raycast(playerPos, posToPoi, rayLength);
                if (hit != null)
                {
                    vertices.Add(new Vector3(hit.point.x, hit.point.y));
                }

                Vector3 bitAfterRotate = bitForward * posToPoi;

                hit = Physics2D.Raycast(playerPos, bitAfterRotate, rayLength);
                if (hit != null)
                {
                    vertices.Add(new Vector3(hit.point.x, hit.point.y));
                }
            }
        }

        triangles.Clear();
        for (int i = 1; i < vertices.Count-1; i++)
        {
            triangles.Add(0);
            triangles.Add(i+1);
            triangles.Add(i);
        }
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(vertices.Count-1);
            
        var meshFilter = this.LOS.GetComponent<MeshFilter>();
        var m = meshFilter.mesh;
        m.Clear();
        m.vertices = vertices.ToArray();
//        m.uv = new Vector2[] { new Vector2 (0, 0), new Vector2 (0, 1), new Vector2(1, 1), new Vector2 (1, 0) };
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();
        meshFilter.mesh = m;
	} 
}
