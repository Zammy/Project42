using System;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExt
{
    public static Vector2 xToVector2(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }

    public static Vector3 xToVector3(this Vector2 vec2)
    {
        return new Vector3(vec2.x, vec2.y, 0);
    }

    public static float xAngleSigned(this Vector3 v1, Vector3 v2, Vector3 rotationAxis)
    {
        return Mathf.Atan2(
            Vector3.Dot(rotationAxis, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }
}
