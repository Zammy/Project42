using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour 
{
    void Start()
    {
        UnityEngine.Cursor.visible = false;
    }
	// Update is called once per frame
	void Update () 
    {
        Vector3 cursorScreenPos = Input.mousePosition;
        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(cursorScreenPos);
        cursorWorldPos.z = -9;

        this.transform.position = cursorWorldPos;
	}
}
