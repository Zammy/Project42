using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour 
{
    void Start()
    {
#if !UNITY_EDITOR
        UnityEngine.Cursor.visible = false;
#endif
    }

    void Update () 
    {
        Vector3 cursorScreenPos = Input.mousePosition;
        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(cursorScreenPos);
        cursorWorldPos.z = -9;

        this.transform.position = cursorWorldPos;
	}
}
