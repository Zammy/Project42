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
        Vector3 diff = (this.Pos - pos).normalized;
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

public class Level : MonoBehaviour 
{
    public GameObject WallPrefab;
    public GameObject GroundPrefab;

    public Transform LevelBase;

    const int SIZE = 15;

    Tile[,] tiles = new Tile[SIZE,SIZE];

	// Use this for initialization
	void Awake () 
    {
        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                GameObject prefab = GroundPrefab;
                if (x == 0 || x == SIZE -1)
                {
                    prefab = WallPrefab;
                }

                if (y == 0 || y == SIZE -1)
                {
                    prefab = WallPrefab;
                }

                tiles[x, y] = InstantiateTile(pos, prefab);
            }   
        }

        Vector3[] extraWalls = new Vector3[]
        {
            new Vector3(10, 10, 0),
            new Vector3(9, 10, 0),
            new Vector3(10, 9, 0),
            new Vector3(7, 3, 0),
        };
        foreach (var wallPos in extraWalls)
        {
            Destroy(tiles[(int)wallPos.x, (int)wallPos.y].gameObject);
            tiles[(int)wallPos.x, (int)wallPos.y] = InstantiateTile(wallPos, WallPrefab);
        }
	}

    Tile InstantiateTile(Vector3 pos, GameObject prefab)
    {
        var tileGo = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        tileGo.transform.SetParent(LevelBase);
        tileGo.transform.localScale = Vector3.one;
        return tileGo.GetComponent<Tile>();
    }

    public Tile GetTileAt(int x, int y)
    {
        try
        {
            return tiles[x, y];
        }
        catch
        {
            return null;
        }
     }

     public List<POI> GetPOIS()
     {
        var walls = new List<Tile>();
        foreach(Tile tile in tiles)
        {
            if (tile.TileTipe == TileType.Wall)
            {
                walls.Add(tile);
            }
        }

        var allPois = new List<POI>();

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

            allPois.AddRange(pois);
        }

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
