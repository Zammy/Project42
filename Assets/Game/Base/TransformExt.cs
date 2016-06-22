using UnityEngine;


public static class TransformExt 
{
    public static void xLookAt(this Transform trans, Vector3 point)
    {
        Vector3 fromPosToPoint = point - trans.position;
        Vector3 dir = fromPosToPoint.normalized;
        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        trans.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    public static RectTransform xToRect(this Transform trans)
    {
        return trans as RectTransform;
    }

    public static T xFindComponentInParents<T>(this Transform trns) 
         where T : Component
    {
        if (trns.parent == null)
            return null;

        var component = trns.parent.GetComponent<T>();
        if (component != null)
        {
            return component;
        }
        return trns.parent.xFindComponentInParents<T>();
    }
}
