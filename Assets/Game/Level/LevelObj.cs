using UnityEngine;

public enum LevelObjType
{
    Wall,
    Ground,
    Building
}

public class LevelObj : MonoBehaviour 
{
    public LevelObjType Type;
    public BoxCollider2D Collider;
}
