using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellularAutomata : MonoBehaviour 
{
    private class TilePosTuple
    {
        public Point Pos;
        public TileType TileType;

        public TilePosTuple(Point pos, TileType type)
        {
            this.Pos = pos;
            this.TileType = type;
        }
    }

    public Level Level;

    public Transform CrewPos;
    
    public float DelayBetweenIterations = 2f;

    public int WallCreateNeighbours = 5;
    public float WallInitialChance = .5f;
    public int Iterations = 2;
    public int RemoveRoomsWithLessThan = 10;

    const int GEN_ZONE_DIST = 25;

    bool generating = false;
    TileType[,] zone = new TileType[Level.DUNG_WIDTH, Level.ZONE_SIZE];

	// Use this for initialization
	void Start () 
    {
        Random.seed = 1;// (int)System.DateTime.Now.Ticks;

        this.GenAddNewZone(true);
	}

    void Update()
    {
        int worldEdgeY = this.Level.ZonesNum * Level.ZONE_SIZE;
        float playerY = this.CrewPos.transform.position.y;

        if (worldEdgeY - playerY < GEN_ZONE_DIST &&
            !generating)
        {
            StartCoroutine( GenAddNewZone() );
        }
    }

    IEnumerator GenAddNewZone(bool isInitial = false)
    {
        generating = true;

        yield return StartCoroutine( this.GenerateWalls(isInitial) );
       
        for (int i = 0; i < Iterations; i++)
        {
            yield return StartCoroutine( this.Iterate(isInitial) );
        }

//        yield return StartCoroutine( this.MakeSureLevelIsPassable( isInitial ) );

        this.Level.AddNewZone(zone);

        generating = false;
    }

    IEnumerator GenerateWalls(bool isInitial)
    {
        for (int x = 0; x < Level.DUNG_WIDTH; x++)
        {
            for (int y = 0; y < Level.ZONE_SIZE; y++)
            {
                if ((x == 0 || x == Level.DUNG_WIDTH-1) ||
                    (isInitial && y == 0) ||
                    Random.value < this.WallInitialChance)
                {
                    zone[x, y] = TileType.Wall;
                }
                else
                {
                    zone[x, y] = TileType.Ground;
                }
            }

            if (!isInitial && x % 20 == 0)
                yield return null;
        }
    }

    IEnumerator Iterate(bool isInitial )
    {
        IList<TilePosTuple> changes = new List<TilePosTuple>();

        for (int x = 1; x < Level.DUNG_WIDTH-1; x++)
        {
            for (int y = 1; y < Level.ZONE_SIZE-1; y++)
            {
                int walls = 0;

                for (int dx = -1; dx < 2; dx++)
                {
                    for (int dy = -1; dy < 2; dy++)
                    {
                        if (this.IsTileOfTypeAt(TileType.Wall, x + dx, y + dy))
                            walls++;
                    }
                }

                walls--;

                if (walls >= WallCreateNeighbours)
                {
                    if (this.IsTileOfTypeAt(TileType.Ground, x, y))
                    {
                        changes.Add(new TilePosTuple(new Point(x, y), TileType.Wall));
                    }
                }
                else
                {
                    if (this.IsTileOfTypeAt(TileType.Wall, x, y))
                    {
                        changes.Add(new TilePosTuple(new Point(x, y), TileType.Ground));
                    }
                }
            }

            if (!isInitial && x % 20 == 0)
                yield return null;
        }


        foreach (var change in changes)
        {
            var pos = change.Pos;
            this.zone[pos.X, pos.Y] = change.TileType;
        }
    }

//    IEnumerator MakeSureLevelIsPassable(bool isInitial)
//    {
//        for (int y = 0; y < Level.ZONE_SIZE-1; y++)
//        {
//            int ground = 0;
//            for (int x = 0; x < Level.DUNG_WIDTH-1; x++)
//            {
//                if (this.IsTileOfTypeAt(TileType.Ground, x, y))
//                {
//                    ground++;
//                }
//                else
//                {
//                    ground--;
//                }
//
//                if (!isInitial && x % 20 == 0)
//                    yield return null;
//
//                if (ground > 1)
//                {
//                    break;
//                }
//            }
//
//            if (ground > 1)
//            {
//                continue;
//            }
//
//            Debug.Log("Had to clear on " + y.ToString());
////
////            for (int i = 0; i < Random.Range(1, 4); i++) 
////            {
////                int x = Random.Range(0, Level.DUNG_WIDTH-1);
////                this.zone[x, y] = TileType.Ground;
////            }
//        }
//
//    }

    bool IsTileOfTypeAt(TileType type, int x, int y)
    {
        if (x < 0) 
            return false;
        if (x >= Level.DUNG_WIDTH) 
            return false;
        if (y >= Level.ZONE_SIZE) 
            return false;

        if (y < 0)
        {
            return Level.IsTileOfTypeAt(type, x, y);
        }
        return this.zone[x, y] == type;

    }
}
