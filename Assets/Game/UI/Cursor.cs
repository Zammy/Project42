using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour 
{
    public static Vector3 CursorPosition;

    Plane xy;

    void Start()
    {
#if !UNITY_EDITOR
        UnityEngine.Cursor.visible = false;
#endif
        xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
    }

    void Update ()
    {
        Vector3 cursorWorldPos = PrespectiveCalculate();
        this.transform.position = cursorWorldPos;
        CursorPosition = cursorWorldPos;
    }

    private Vector3 PrespectiveCalculate()
    {
        Vector3 cursorScreenPos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(cursorScreenPos);
        
        float distance;
        xy.Raycast(ray, out distance);
        Vector3 cursorWorldPos = ray.GetPoint(distance);
        return cursorWorldPos;
    }
}
