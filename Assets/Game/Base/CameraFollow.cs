using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
    public float Z = -10f;

	// Update is called once per frame
	void Update () 
    {
        Camera.main.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Z);
	}
}
