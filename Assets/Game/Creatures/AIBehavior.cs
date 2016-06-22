using UnityEngine;
using System.Collections;

public abstract class AIBehavior : MonoBehaviour
{
    public Transform CreatureTransform;


    public static Vector3[] Empty = new Vector3[0];

    public abstract Vector3[] GetInterest();
    public abstract Vector3[] GetDanger();

    public float Strength = 1f;
}
