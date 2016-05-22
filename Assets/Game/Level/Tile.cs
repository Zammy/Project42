using UnityEngine;
using System.Collections;

public enum TileType
{
    Wall,
    Ground
}

public class Tile : MonoBehaviour 
{
    public TileType TileTipe;

    private SpriteRenderer sprite;

    void Start()
    {
        this.sprite = this.GetComponentInChildren<SpriteRenderer>();
    }

    public bool DebugHighlight
    {
        get
        {
            return this.sprite.color == Color.red;
        }

        set
        {
            if (value)
            {
                this.sprite.color = Color.red;
            }
            else
            {
                this.sprite.color = Color.white;
            }
        }
    }
}
