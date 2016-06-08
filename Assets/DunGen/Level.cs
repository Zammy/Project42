using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DunGen
{
    public class Level : MonoBehaviour
    {
        public GameObject WallPrefab;
        public GameObject GroundPrefab;

        public Transform LevelBase;

        public int SIZE = 50;

        protected Tile[,] tiles;

        protected List<GameObject> creatures = new List<GameObject>();

        void Awake()
        {
            this.tiles = new Tile[SIZE, SIZE];

            if (this.transform.childCount > 0)
            {
                this.GetTilesFrom(this.transform);
            }
        }

        public void AddRoom(int addX, int addY, int size)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Vector3 pos = new Vector3(addX + x, addX + y, 0);
                    GameObject prefab = GroundPrefab;
                    if (x == 0 || x == size - 1)
                    {
                        prefab = WallPrefab;
                    }

                    if (y == 0 || y == size - 1)
                    {
                        prefab = WallPrefab;
                    }

                    tiles[x + addX, y + addY] = InstantiateTile(pos, prefab);
                }
            }
        }

        public void RemoveTileAt(Point pos)
        {
            try
            {
                var tile = this.tiles[pos.X, pos.Y];
                if (tile != null)
                {
                    Destroy(tile.gameObject);
                }
            }
            catch
            { }
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
            this.tiles[pos.X, pos.Y] = InstantiateTile(new Vector3(pos.X, pos.Y, 0), prefab);
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

        public Tile GetTileAt(Point p)
        {
            return GetTileAt(p.X, p.Y);
        }

        public bool IsTileOfTypeAt(TileType type, int x, int y)
        {
            var tile = GetTileAt(x, y);
            if (tile == null)
                return false;

            return tile.TileTipe == type;
        }

        public bool IsTileOfTypeAt(TileType type, Point p)
        {
            return IsTileOfTypeAt(type, p.X, p.Y);
        }

        public void GetTilesFrom(Transform t)
        {
            foreach (Transform tileTrns in t)
            {
                var pos = tileTrns.position;
                this.tiles[(int)pos.x, (int)pos.y] = tileTrns.gameObject.GetComponent<Tile>();
            }
        }

        public bool IsPassable(Point p)
        {
            var tile = GetTileAt(p);
            if (tile == null)
                return false;

            return tile.TileTipe != TileType.Wall;
        }

        public bool IsPassable(Vector3 pos)
        {
            Point p = pos.xToPoint();
            return this.IsPassable(p);
        }

        public List<Tile> GetImpassableAround(Vector3 position, int searchRange)
        {
            List<Tile> tiles = new List<Tile>();

            int intX = Mathf.RoundToInt(position.x);
            int intY = Mathf.RoundToInt(position.y);

            for (int x = intX - searchRange; x < intX + searchRange; x++)
            {
                if (x < 0 || x >= this.SIZE)
                    continue;

                for (int y = intY - searchRange; y < intY + searchRange; y++)
                {
                    if (y < 0 || y >= this.SIZE)
                        continue;

                    Tile tile = this.GetTileAt(x, y);
                    if (tile == null)
                        continue;

                    if (tile.TileTipe == TileType.Wall)
                    {
                        tiles.Add(tile);
                    }
                }
            }

            return tiles;
        }

        public List<GameObject> GetCreaturesAround(Transform trans, float searchRange)
        {
            var creaturesAround = new List<GameObject>();
            float sqredRng = searchRange * searchRange;
            for (int i = 0; i < creatures.Count; i++)
            {
                var creature = creatures[i];
                if (creature != trans.gameObject &&
                     (creature.transform.position - trans.position).sqrMagnitude < sqredRng)
                {
                    creaturesAround.Add(creature);
                }
            }

            return creaturesAround;
        }

        public void RemoveCreature(GameObject creature)
        {
            this.creatures.Remove(creature);
        }

        protected Tile InstantiateTile(Vector3 pos, GameObject prefab)
        {
            var tileGo = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
            tileGo.transform.SetParent(LevelBase);
            tileGo.transform.localScale = Vector3.one;
            return tileGo.GetComponent<Tile>();
        }
    }
}