using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum POINormal
{
    NE,
    NW,
    SE,
    SW
}

public class POI //point of interest
{
    public Vector3 Pos;
    public POINormal Normal;

    public POI(Vector3 pos, POINormal normal)
    {
        Pos = pos;
        Normal = normal;
    }

    public float AngleTo(Vector3 pos)
    {
        Vector3 diff = (this.Pos - pos);//.normalized;
        return Quaternion.FromToRotation(Vector3.up, diff).eulerAngles.z;
    }

    private static Vector3 nw = new Vector3(-0.7f, +0.7f);
    private static Vector3 ne = new Vector3(+0.7f, +0.7f);
    private static Vector3 se = new Vector3(+0.7f, -0.7f);
    private static Vector3 sw = new Vector3(-0.7f, -0.7f);

    public Vector3 GetNormal()
    {
        switch (Normal)
        {
            case POINormal.NE:
                return ne;
            case POINormal.NW:
                return nw;
            case POINormal.SE:
                return se;
            case POINormal.SW:
                return sw;
            default:
                throw new UnityException("This should not happen!");
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is POI)
        {
            POI other = (POI) obj;
            return (this.Pos - other.Pos).sqrMagnitude < 0.01f
                &&  this.Normal == other.Normal;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Pos.GetHashCode() + 17 * (int) Normal;
    }
}

public class LevelExt : Level
{
    public BoxCollider2D TopSightStopper;
    public BoxCollider2D BottomSightStopper;
    public BoxCollider2D RightSightStopper;
    public BoxCollider2D LeftSightStopper;

    public GameObject DebugPoint;

    List<POI> pointsOfInterest = new List<POI>();

    protected override void Awake()
    {
        base.Awake();
    }


    int debugIndex = 0;
    List<GameObject> debugPoints = new List<GameObject>();
    void AddDebugPoint(Vector3 pos)
    {
        GameObject pointGo;
        if (debugIndex >= debugPoints.Count )
        {
            pointGo = (GameObject) Instantiate( DebugPoint );
            debugPoints.Add(pointGo);
        }
        else
        {
            pointGo = debugPoints[debugIndex];
        }
        pointGo.transform.position = pos;
//        pointGo.transform.localScale = Vector3.one;
        pointGo.SetActive(true);
        debugIndex++;
    }

    public List<POI> GetPOIS()
    {
        debugIndex = 0;

        foreach (var point in debugPoints)
        {
            point.gameObject.SetActive(false);
        }

        pointsOfInterest.Clear();

        Bounds cameraBounds = Camera.main.CalcOrthographicBounds();

        //bottom left of camera
        pointsOfInterest.Add(new POI( cameraBounds.min, POINormal.NE ));
        //bottom right of camera
        Vector3 bottomRight = cameraBounds.min;
        bottomRight.x += cameraBounds.size.x;
        pointsOfInterest.Add(new POI( bottomRight, POINormal.NW ));
        //top right
        pointsOfInterest.Add(new POI( cameraBounds.max, POINormal.SW ));
        //top left
        Vector3 topLeft = cameraBounds.min;
        topLeft.y += cameraBounds.size.y;
        pointsOfInterest.Add(new POI( topLeft, POINormal.SE ));


        var walls = new List<Tile>();
        foreach(Tile tile in this.tiles)
        {
            if (tile == null) continue;
            if (tile.TileTipe != TileType.Wall) continue;

            var collider = tile.GetComponent<BoxCollider2D>();

            if (collider.bounds.Intersects(cameraBounds))
            {
                walls.Add(tile);
            }
        }

        foreach (var wall in walls)
        {
            Vector3 wallPos = wall.transform.position;
            var pois = new List<POI>()
            {
                new POI( new Vector3( wallPos.x + 0.5f, wallPos.y + 0.5f, 0), POINormal.NE ),
                new POI( new Vector3( wallPos.x + 0.5f, wallPos.y - 0.5f, 0), POINormal.SE ),
                new POI( new Vector3( wallPos.x - 0.5f, wallPos.y - 0.5f, 0), POINormal.SW ),
                new POI( new Vector3( wallPos.x - 0.5f, wallPos.y + 0.5f, 0), POINormal.NW ),
            };

            bool toNorth = false;
            bool toSouth = false;
            bool toEast = false;
            bool toWest = false;

            Tile tile = GetTileAt( (int)wallPos.x, (int)wallPos.y + 1 );
            toNorth = (tile != null && tile.TileTipe == TileType.Wall);

            tile = GetTileAt( (int)wallPos.x, (int)wallPos.y - 1 );
            toSouth = (tile != null && tile.TileTipe == TileType.Wall);

            tile = GetTileAt( (int)wallPos.x + 1, (int)wallPos.y );
            toEast = (tile != null && tile.TileTipe == TileType.Wall);

            tile = GetTileAt( (int)wallPos.x - 1, (int)wallPos.y );
            toWest = (tile != null && tile.TileTipe == TileType.Wall);

            if ((toEast && !toNorth) || (!toEast && toNorth))
            {
                RemovePOI(pois, POINormal.NE);
            }

            if ((toEast && !toSouth) || (!toEast && toSouth))
            {
                RemovePOI(pois, POINormal.SE);
            }

            if ((toSouth && !toWest) || (!toSouth && toWest))
            {
                RemovePOI(pois, POINormal.SW);
            }

            if ((toWest && !toNorth) || (!toWest && toNorth))
            {
                RemovePOI(pois, POINormal.NW);
            }

            var wallCollider = wall.GetComponent<BoxCollider2D>();

            if (TopSightStopper.bounds.Intersects(wallCollider.bounds))
            {
                Vector3 pointPos = wall.transform.position;
                if (!toEast)
                {
                    var pos = new Vector3(pointPos.x + 0.5f, TopSightStopper.transform.position.y, 0);
//                    AddDebugPoint(pos);
                    pois.Add(new POI(pos, POINormal.SE));
                }
                if (!toWest)
                {
                    var pos = new Vector3(pointPos.x - 0.5f, TopSightStopper.transform.position.y, 0);
//                    AddDebugPoint(pos);
                    pois.Add(new POI(pos, POINormal.SW));
                }
            }
            
            if (BottomSightStopper.bounds.Intersects(wallCollider.bounds))
            {
                Vector3 pointPos = wall.transform.position;
                if (!toEast)
                {
                    var pos = new Vector3(pointPos.x + 0.5f, BottomSightStopper.transform.position.y, 0);
//                    AddDebugPoint(pos);
                    pois.Add(new POI(pos, POINormal.NE));
                }
                if (!toWest)
                {
                    var pos = new Vector3(pointPos.x - 0.5f, BottomSightStopper.transform.position.y, 0);
//                    AddDebugPoint(pos);
                    pois.Add(new POI(pos, POINormal.NW));
                }
            }

            if (RightSightStopper.bounds.Intersects(wallCollider.bounds))
            {
                Vector3 pointPos = wall.transform.position;
                if (!toNorth)
                {
                    var pos = new Vector3(RightSightStopper.transform.position.x, wall.transform.position.y + 0.5f, 0);
//                    AddDebugPoint(pos);
                    pois.Add(new POI(pos, POINormal.NW));
                }
                if (!toSouth)
                {
                    var pos = new Vector3(RightSightStopper.transform.position.x, wall.transform.position.y - 0.5f, 0);
//                    AddDebugPoint(pos);
                    pois.Add(new POI(pos, POINormal.SW));
                }
            }

            if (LeftSightStopper.bounds.Intersects(wallCollider.bounds))
            {
                Vector3 pointPos = wall.transform.position;
                if (!toNorth)
                {
                    var pos = new Vector3(LeftSightStopper.transform.position.x, wall.transform.position.y + 0.5f, 0);
//                    AddDebugPoint(pos);
                    pois.Add(new POI(pos, POINormal.NE));
                }
                if (!toSouth)
                {
                    var pos = new Vector3(LeftSightStopper.transform.position.x, wall.transform.position.y - 0.5f, 0);
//                    AddDebugPoint(pos);
                    pois.Add(new POI(pos, POINormal.SE));
                }
            }

            pointsOfInterest.AddRange(pois);
        }

        return pointsOfInterest;
     }

     static void RemovePOI(List<POI> pois, POINormal normal)
     {
        foreach (var poi in pois)
        {
            if (poi.Normal == normal)
            {
                pois.Remove(poi);
                break;
            }
        }   
     }
}
