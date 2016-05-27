using UnityEngine;
using System.Collections;

public abstract class AIBehavior : MonoBehaviour
{
    public static Vector2[] Empty = new Vector2[0];

    public abstract Vector2[] GetInterest();
    public abstract Vector2[] GetDanger();

    public float Strength = 1f;

    protected Transform creatureTransform
    {
        get
        {
            return this.transform.parent.transform;
        }
    }
}
