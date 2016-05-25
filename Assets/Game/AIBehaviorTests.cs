using UnityEngine;
using System.Collections;

public class AIBehaviorTests : MonoBehaviour 
{
    public Seek[] Seeks;

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
            foreach (var seek in Seeks)
            {
                seek.Goal = new Vector2(cursorWorldPos.x + Random.Range(-0.5f, +0.5f), cursorWorldPos.y + Random.Range(-0.5f, +0.5f));
            }
        }
	}
}
