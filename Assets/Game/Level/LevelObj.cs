using UnityEngine;

public enum LevelObjType
{
    Wall,
    Ground,
    Building,
    Obastacle
}

public class LevelObj : MonoBehaviour 
{
    public LevelObjType Type;
    public BoxCollider2D Collider;

    CharacterHealth charHealth;

    void Start()
    {
        if (Type == LevelObjType.Obastacle)
        {
            charHealth = this.GetComponent<CharacterHealth>();
            if (charHealth != null)
            {
                charHealth.CharacterDied += OnDestroyed;
            }
        }
    }

    void OnDestroy()
    {
        if (charHealth != null)
        {
            charHealth.CharacterDied -= OnDestroyed;
        }
    }

    private void OnDestroyed(GameObject _)
    {
        Level.Instance.RemoveObject(this);
        Destroy(this.gameObject);
    }
}
