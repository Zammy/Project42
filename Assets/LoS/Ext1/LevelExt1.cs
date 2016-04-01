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

public class LevelExt1 : Level
{
    const int TEST_ROOM_SIZE = 15;

    protected override void Awake()
    {
        base.Awake();

//        this.AddRoom(0, 0, TEST_ROOM_SIZE);
//
//        Point[] extraWalls = new Point[]
//        {
//            new Point ( 10, 10 ),
//            new Point (  9, 10 ),
//            new Point (  10, 9 ),
//            new Point (  10, 6 ),
//            new Point (  7, 3 ),
//            new Point (  1, 1 ),
//            new Point (  4, 3 ),
//
//        };
//        foreach (var wallPos in extraWalls)
//        {
//            this.AddTile(TileType.Wall, wallPos);
//        }
    }

    public List<POI> GetPOIS()
    {
//        var walls = new List<Tile>();
//        foreach(Tile tile in this.tiles)
//        {
//            if (tile == null) continue;
//
//            if (tile.TileTipe == TileType.Wall)
//            {
//                walls.Add(tile);
//            }
//        }
//
        var allPois = new List<POI>();
//
//        foreach (var wall in walls)
//        {
//            Vector3 wallPos = wall.transform.position;
//            var pois = new List<POI>()
//            {
//                new POI( new Vector3( wallPos.x + 0.5f, wallPos.y + 0.5f, 0), POINormal.NE ),
//                new POI( new Vector3( wallPos.x + 0.5f, wallPos.y - 0.5f, 0), POINormal.SE ),
//                new POI( new Vector3( wallPos.x - 0.5f, wallPos.y - 0.5f, 0), POINormal.SW ),
//                new POI( new Vector3( wallPos.x - 0.5f, wallPos.y + 0.5f, 0), POINormal.NW ),
//            };
//
//            bool toNorth = false;
//            bool toSouth = false;
//            bool toEast = false;
//            bool toWest = false;
//
//            Tile tile = GetTileAt( (int)wallPos.x, (int)wallPos.y + 1 );
//            toNorth = (tile != null && tile.TileTipe == TileType.Wall);
//
//            tile = GetTileAt( (int)wallPos.x, (int)wallPos.y - 1 );
//            toSouth = (tile != null && tile.TileTipe == TileType.Wall);
//
//            tile = GetTileAt( (int)wallPos.x + 1, (int)wallPos.y );
//            toEast = (tile != null && tile.TileTipe == TileType.Wall);
//
//            tile = GetTileAt( (int)wallPos.x - 1, (int)wallPos.y );
//            toWest = (tile != null && tile.TileTipe == TileType.Wall);
//
//            if ((toEast && !toNorth) || (!toEast && toNorth))
//            {
//                RemovePOI(pois, POINormal.NE);
//            }
//
//            if ((toEast && !toSouth) || (!toEast && toSouth))
//            {
//                RemovePOI(pois, POINormal.SE);
//            }
//
//            if ((toSouth && !toWest) || (!toSouth && toWest))
//            {
//                RemovePOI(pois, POINormal.SW);
//            }
//
//            if ((toWest && !toNorth) || (!toWest && toNorth))
//            {
//                RemovePOI(pois, POINormal.NW);
//            }
//
//            allPois.AddRange(pois);
//        }

        return allPois;
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
