using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour 
{
    public GameObject WallPrefab;
    public GameObject GroundPrefab;

    public Transform LevelBase;

    public const int ZONE_SIZE = 50;
    public const int DUNG_WIDTH = 30;

    public int ZonesNum
    {
        get
        {
            return zones.Count;
        }
    }

    List<Tile[,]> zones = new List<Tile[,]>();

    Dictionary<TileType, GameObject> tileTypeToPrefab;

	// Use this for initialization
	protected virtual void Awake ()
    {
        this.tileTypeToPrefab = new Dictionary<TileType, GameObject>()
        {
            { TileType.Wall, WallPrefab },
            { TileType.Ground, GroundPrefab }  
        };
	}

    public void AddNewZone(TileType[,] zone)
    {
        var zoneTiles = new Tile[zone.GetLength(0), zone.GetLength(1)];

        for (int x = 0; x < zone.GetLength(0); x++)
        {
            for (int y = 0; y < zone.GetLength(1); y++)
            {
                var type = zone[x, y];
                var pos = new Vector3(x, y + this.ZonesNum * ZONE_SIZE);
                zoneTiles[x, y] = InstantiateTile(pos, this.tileTypeToPrefab[type]);
            }
        }

        this.zones.Add(zoneTiles);
    }

//    public void AddRoom(int addX, int addY, int size)
//    {
//        for (int x = 0; x < size; x++)
//        {
//            for (int y = 0; y < size; y++)
//            {
//                Vector3 pos = new Vector3(addX + x, addX + y, 0);
//                GameObject prefab = GroundPrefab;
//                if (x == 0 || x == size -1)
//                {
//                    prefab = WallPrefab;
//                }
//
//                if (y == 0 || y == size -1)
//                {
//                    prefab = WallPrefab;
//                }
//
//                tiles[x+addX, y+addY] = InstantiateTile(pos, prefab);
//            }
//        }
//    }

    public void RemoveTileAt(Point pos)
    {
        try
        {
            var tile = GetTileAt(pos.X, pos.Y);
            if (tile != null)
            {
                Destroy(tile.gameObject);
            }
        }
        catch
        {}
    }

    public void AddTile(TileType type, Point pos)
    {
        RemoveTileAt(pos);
        GameObject prefab;
        switch (type)
        {
            case TileType.Ground:
            {
                prefab = GroundPrefab;
                break;
            }
            case TileType.Wall:
            {
                prefab = WallPrefab;
                break;
            }
            default:
                throw new UnityException("This should not happen!");
        }
        this.SetTileAt(InstantiateTile(new Vector3(pos.X, pos.Y, 0), prefab), pos);
    }


    public Tile GetTileAt(int x, int y)
    {
        try
        {
            int zoneIndex = y / ZONE_SIZE;
            int zoneY = y % ZONE_SIZE;
            var zone = this.zones[zoneIndex];

            return zone[x, zoneY];
        }
        catch
        {
            return null;
        }
     }

     public Tile GetTileAt(Point p)
     {
        return GetTileAt(p.X, p.Y);
     }

     public bool IsTileOfTypeAt(TileType type, int x, int y)
     {
        var tile = GetTileAt(x,y);
        if (tile == null)
            return false;

        return tile.TileTipe == type;
     }

    public bool IsTileOfTypeAt(TileType type, Point p)
    {
        return IsTileOfTypeAt(type, p.X , p.Y);
    }

    protected void SetTileAt(Tile tile, Point p)
    {
        int zoneIndex = p.Y / ZONE_SIZE;
        int zoneY = p.Y % ZONE_SIZE;
        var zone = this.zones[zoneIndex];

        zone[p.X, zoneY] = tile;
    }

    protected Tile InstantiateTile(Vector3 pos, GameObject prefab)
    {
        var tileGo = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        tileGo.transform.SetParent(LevelBase);
        tileGo.transform.localScale = Vector3.one;
        return tileGo.GetComponent<Tile>();
    }
}
