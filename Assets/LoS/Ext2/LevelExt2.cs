using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SegmentNormal
{
    Up,
    Right,
    Down,
    Left
}

public class EndPoint
{
    public int Index;
    public Segment Segment;

    public float AngleTo(Vector3 pos)
    {
        var endPoint = this.GetPoint();
        Vector3 diff = (endPoint - pos).normalized;
        return Quaternion.FromToRotation(Vector3.up, diff).eulerAngles.z;
    }

    public Vector3 GetPoint()
    {
        var pos2d = Segment.Points[Index];
        return new Vector3(pos2d.x, pos2d.y);
    }
}

public class Segment
{
    public Vector2[] Points = new Vector2[2];
    public SegmentNormal Normal;

    private EndPoint[] EndPoints;

    public Segment (SegmentNormal normal, Vector2 start)
    {
        Normal = normal;
        Points[0] = start;
    }

    public EndPoint[] GetEndPoints()
    {
        if (EndPoints == null)
        {
            this.EndPoints = new EndPoint[]
            {
                new EndPoint()
                {
                    Index = 0,
                    Segment = this
                },
                new EndPoint()
                {
                    Index = 1,
                    Segment = this
                }
            };
        }

        return EndPoints;
    }

    public Vector3 GetNormal()
    {
        switch (Normal)
        {
            case SegmentNormal.Up:
                return Vector3.up;
            case SegmentNormal.Right:
                return Vector3.right;
            case SegmentNormal.Down:
                return Vector3.down;
            case SegmentNormal.Left:
                return Vector3.left;
            default:
                throw new UnityException("This should not happen!");
        }
    }
}

public class LevelExt2 : Level 
{
    List<Segment> segments = new List<Segment>();

    void Start()
    {
    }

//    void Update()
//    {   
//        foreach (var seg in segments)
//        {
//            Vector3 diff = seg.Points[1] - seg.Points[0];
//            Debug.DrawRay(seg.Points[0], diff, Color.green, diff.magnitude);
//        }
//    }

    public List<Segment> GetSegments()
    {
        var walls = new List<Tile>();
        foreach(Tile tile in tiles)
        {
            if (tile.TileTipe == TileType.Wall)
            {
                walls.Add(tile);
            }
        }

        var startedSegments = new List<Segment>();
        System.Func<Vector3, SegmentNormal, Segment> getSeg = (wallPos, normal) =>
        {
            float wallValue;
            switch (normal) 
            {
                case SegmentNormal.Up:
                {
                    wallValue = wallPos.y + 0.5f;
                    break;
                }
                case SegmentNormal.Down:
                {
                    wallValue = wallPos.y - 0.5f;
                    break;
                }
                case SegmentNormal.Left:
                {
                    wallValue = wallPos.x - 0.5f;
                    break;
                }
                case SegmentNormal.Right:
                {
                    wallValue = wallPos.x + 0.5f;
                    break;
                }
                default:
                    throw new UnityException("This should not happen!");
            }

            foreach (var seg in startedSegments)
            {
                if (normal != seg.Normal)
                {
                    continue;
                }

                if (normal == SegmentNormal.Up || normal == SegmentNormal.Down)
                {
                    if ( Mathf.Abs(seg.Points[0].y - wallValue) > 0.1f)
                    {
                        continue;
                    }
                }
                else
                {
                    if ( Mathf.Abs(seg.Points[0].x - wallValue) > 0.1f)
                    {
                        continue;
                    }
                }

                return seg;
            }

            return null;
        };


        System.Action<Segment, Vector2> finishSeg = (Segment seg, Vector2 point) =>
        {
            seg.Points[1] = point;
            startedSegments.Remove(seg);
            segments.Add(seg);
        };

        var normals = new SegmentNormal[] { SegmentNormal.Up, SegmentNormal.Right, SegmentNormal.Down, SegmentNormal.Left };
        foreach (var wall in walls)
        {
            Vector3 wallPos = wall.transform.position;
            int x = (int)wallPos.x;
            int y = (int)wallPos.y;

            foreach (SegmentNormal normal in normals)
            {
                Segment seg = getSeg(wallPos, normal);
                if (normal == SegmentNormal.Up)
                {
                    if (seg == null)
                    {
                        if (!IsWallAt(x, y+1))
                        {
                            seg = new Segment(normal, new Vector2(wallPos.x - 0.5f, wallPos.y + 0.5f));
                            startedSegments.Add(seg);
                        }
                    }

                    if (seg != null)
                    {
                        if (IsWallAt(x, y+1))
                        {
                            finishSeg(seg, new Vector2(wallPos.x - 0.5f, wallPos.y + 0.5f));

                        }
                        else if (!IsWallAt(x+1, y))
                        {
                            finishSeg(seg, new Vector2(wallPos.x + 0.5f, wallPos.y + 0.5f));
                        }
                    }
                }
                else if (normal == SegmentNormal.Right)
                {
                    if (seg == null)
                    {
                        if (!IsWallAt(x+1, y))
                        {
                            seg = new Segment(normal, new Vector2(wallPos.x + 0.5f, wallPos.y - 0.5f));
                            startedSegments.Add(seg);
                        }
                    }

                    if (seg != null)
                    {
                        if (IsWallAt(x+1, y))
                        {
                            finishSeg(seg, new Vector2(wallPos.x + 0.5f, wallPos.y - 0.5f));

                        }
                        else if (!IsWallAt(x, y+1))
                        {
                            finishSeg(seg, new Vector2(wallPos.x + 0.5f, wallPos.y + 0.5f));
                        }
                    }
                }
                else if (normal == SegmentNormal.Down)
                {
                    if (seg == null)
                    {
                        if (!IsWallAt(x, y-1))
                        {
                            seg = new Segment(normal, new Vector2(wallPos.x - 0.5f, wallPos.y - 0.5f));
                            startedSegments.Add(seg);
                        }
                    }

                    if (seg != null)
                    {
                        if (IsWallAt(x, y-1))
                        {
                            finishSeg(seg, new Vector2(wallPos.x - 0.5f, wallPos.y - 0.5f));

                        }
                        else if (!IsWallAt(x+1, y))
                        {
                            finishSeg(seg, new Vector2(wallPos.x + 0.5f, wallPos.y - 0.5f));
                        }
                    }
                }
                else
                {
                    if (seg == null)
                    {
                        if (!IsWallAt(x-1, y))
                        {
                            seg = new Segment(normal, new Vector2(wallPos.x - 0.5f, wallPos.y - 0.5f));
                            startedSegments.Add(seg);
                        }
                    }

                    if (seg != null)
                    {
                        if (IsWallAt(x-1, y))
                        {
                            finishSeg(seg, new Vector2(wallPos.x - 0.5f, wallPos.y - 0.5f));

                        }
                        else if (!IsWallAt(x, y+1))
                        {
                            finishSeg(seg, new Vector2(wallPos.x - 0.5f, wallPos.y + 0.5f));
                        }
                    }
                }
            }
        }

        return this.segments;
    }
}
