using UnityEngine;
using System.Collections;

public class AIBehaviorTests : MonoBehaviour 
{
    public Seek Seek;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 cursorScreenPos = Input.mousePosition;
        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(cursorScreenPos);
        cursorWorldPos.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            Seek.Goal = cursorWorldPos;
        }
	}
}
