﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LineOfSightDrawer : MonoBehaviour 
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

    public Transform LOS { get; set;}

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    const float GOES_THROUGH = 0.7071f;
    const float IGNORE = -0.7071f;

    void FixedUpdate()
    {
        this.LOS.position = new Vector3(this.transform.position.x, this.transform.position.y, -1f);
    }

	// Use this for initialization
	public void GenerateLOSMesh (List<POI> pois)
    {
        int mask = LayerMask.GetMask(new string[] { "SightBlock" } );

        float rayLength = /*Camera.main.orthographicSize * 2 * Camera.main.aspect + */ 100f;
        Quaternion bitBack = Quaternion.AngleAxis(-0.1f, Vector3.forward);
        Quaternion bitForward = Quaternion.AngleAxis(0.1f, Vector3.forward);

        Vector3 playerPos = this.transform.position;
        vertices.Clear();
        vertices.Add(Vector3.zero);

        pois.Sort(new POIComparer(playerPos));

        for (int i = 0; i < pois.Count; i++)
        {
            POI poi = pois[i];

            Vector3 fromPOItoPos = playerPos - poi.Pos;
            Vector3 fromPOItoPosDir = fromPOItoPos.normalized;
            float dot = Vector3.Dot(fromPOItoPosDir, poi.GetNormal());
            if (dot > GOES_THROUGH)
            {
                RaycastHit2D hit = Physics2D.Raycast(playerPos, -fromPOItoPos, rayLength, mask);
                if (hit != null)
                {
                    Vector3 point = new Vector3(hit.point.x, hit.point.y);// + fromPOItoPosDir * ExtraMargin;
                    vertices.Add(point - playerPos);
                }
            }
            else if ( dot > IGNORE && dot < GOES_THROUGH )
            {
                Vector3 posToPoi = -fromPOItoPos;

                Vector3 bitBeforeRotated = bitBack * posToPoi;

                RaycastHit2D hit = Physics2D.Raycast(playerPos, bitBeforeRotated, rayLength, mask);
                if (hit != null)
                {
                    Vector3 point = new Vector3(hit.point.x, hit.point.y);// + fromPOItoPosDir * ExtraMargin;
                    vertices.Add(point- playerPos);
                }

                hit = Physics2D.Raycast(playerPos, posToPoi, rayLength, mask);
                if (hit != null)
                {
                    Vector3 point = new Vector3(hit.point.x, hit.point.y);// + fromPOItoPosDir * ExtraMargin;
                    vertices.Add(point- playerPos);
                }

                Vector3 bitAfterRotate = bitForward * posToPoi;

                hit = Physics2D.Raycast(playerPos, bitAfterRotate, rayLength, mask);
                if (hit != null)
                {
                    Vector3 point = new Vector3(hit.point.x, hit.point.y);// + fromPOItoPosDir * ExtraMargin;
                    vertices.Add(point - playerPos);
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