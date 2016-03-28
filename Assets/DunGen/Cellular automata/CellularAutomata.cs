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
    
    public float DelayBetweenIterations = 2f;

    public int WallCreateNeighbours = 5;
    public float WallInitialChance = .5f;
    public int Iterations = 2;
    public int RemoveRoomsWithLessThan = 10;

	// Use this for initialization
	void Start () 
    {
        Level.AddRoom(0, 0, Level.SIZE);

        Random.seed =  (int)System.DateTime.Now.Ticks;

	    StartCoroutine(this.Generate());
	}

    void GenerateInitialWalls()
    {
        for (int x = 0; x < Level.SIZE-1; x++)
        {
            for (int y = 0; y < Level.SIZE-1; y++)
            {
                if (UnityEngine.Random.value < WallInitialChance)
                {
                    this.Level.AddTile(TileType.Wall,new Point(x, y));
                }
            }
        }

    }

    void Iterate()
    {
        IList<TilePosTuple> changes = new List<TilePosTuple>();

        for (int x = 1; x < Level.SIZE-1; x++)
        {
            for (int y = 1; y < Level.SIZE-1; y++)
            {
                int walls = 0;

                for (int dx = -1; dx < 2; dx++)
                {
                    for (int dy = -1; dy < 2; dy++)
                    {
                        if (Level.IsTileOfTypeAt(TileType.Wall, x + dx, y + dy))
                            walls++;
                    }
                }

                walls--;

                if (walls >= WallCreateNeighbours)
                {
                    if (Level.IsTileOfTypeAt(TileType.Ground, x, y))
                    {
                        changes.Add(new TilePosTuple(new Point(x, y), TileType.Wall));
                    }
                }
                else
                {
                    if (Level.IsTileOfTypeAt(TileType.Wall, x, y))
                    {
                        changes.Add(new TilePosTuple(new Point(x, y), TileType.Ground));
                    }
                }
            }
        }


        foreach (var change in changes)
        {
            Level.AddTile(change.TileType, change.Pos);
        }
    }



    void AddToRoom(List<Point> grounds, List<Point> room, Point p)
    {
        if (room.Contains(p))
            return;

        grounds.Remove(p);
        room.Add(p);

        var dirs = new Point[]
        {
            new Point(p.X, p.Y + 1),
            new Point(p.X, p.Y - 1),
            new Point(p.X-1, p.Y),
            new Point(p.X+1, p.Y)
        };

        foreach (var point in dirs)
        {
            var tile = Level.GetTileAt(point);
            if (tile != null && tile.TileTipe == TileType.Ground)
            {
                AddToRoom(grounds, room, point);
            } 
        }
    }

    List<List<Point>> FindRooms()
    {
        var rooms = new List<List<Point>>();
//
//        System.Func<Point, bool> isInRoom = (p) =>
//        {
//            foreach (var room in rooms) 
//            {
//                if (room.Contains(p))
//                {
//                    return true;
//                }
//            }
//            return false ; 
//        };
//
        var grounds = new List<Point>();
        for (int x = 1; x < Level.SIZE-1; x++)
        {
            for (int y = 1; y < Level.SIZE-1; y++)
            {
                Point p = new Point(x, y);
                if (Level.IsTileOfTypeAt(TileType.Ground, p))
                {
                    grounds.Add(p);
                }
            }
        }

        while(grounds.Count > 0)
        {
            var room = new List<Point>();
            AddToRoom(grounds, room, grounds[0]);
            rooms.Add(room);
        }

        return rooms;
    }

    IEnumerator Generate()
    {
        this.GenerateInitialWalls();

        yield return new WaitForSeconds(DelayBetweenIterations);

        for (int i = 0; i < Iterations; i++)
        {
            Iterate();

            yield return new WaitForSeconds(DelayBetweenIterations);
        }

        var rooms = FindRooms();

        yield return new WaitForSeconds(DelayBetweenIterations);

        foreach (var room in rooms)
        {
            if (room.Count < RemoveRoomsWithLessThan)
            {
                foreach (var p in room)
                {
                    Level.AddTile(TileType.Wall, p);
                }
            }
        }
//
//        Debug.Log("Rooms " + rooms.Count);

    }
}
