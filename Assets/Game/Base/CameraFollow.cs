using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
    {
        Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
	}
}
