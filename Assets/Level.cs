using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour 
{
    public GameObject WallPrefab;
    public GameObject GroundPrefab;

    public Transform LevelBase;

    const int SIZE = 15;

    protected Tile[,] tiles = new Tile[SIZE,SIZE];

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
            new Vector3(10, 7, 0),
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

     protected bool IsWallAt(int x, int y)
     {
        var tile = GetTileAt(x,y);
        if (tile == null)
            return false;

        return tile.TileTipe == TileType.Wall;
     }
}
