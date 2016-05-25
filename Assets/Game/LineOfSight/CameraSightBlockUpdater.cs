using UnityEngine;
using System.Collections;

public class CameraSightBlockUpdater : MonoBehaviour 
{
    public Transform Top;
    public Transform Right;
    public Transform Left;
    public Transform Bottom;

    Camera kamera;

    void Start()
    {
        this.kamera = this.GetComponent<Camera>();
    }

	// Update is called once per frame
	void FixedUpdate () 
    {
	    float orthoSize = this.kamera.orthographicSize;
        float halfWidth = orthoSize * this.kamera.aspect;
        
        Vector3 pos = this.Top.position;
        pos.y = kamera.transform.position.y + orthoSize;
        pos.z = 0f;
        this.Top.position = pos;

        pos = this.Bottom.position;
        pos.y = kamera.transform.position.y - orthoSize;
        pos.z = 0f;
        this.Bottom.position = pos;

        pos = this.Right.position;
        pos.x = kamera.transform.position.x + halfWidth;
        pos.z = 0f;
        this.Right.position = pos;

        pos = this.Left.position;
        pos.x = kamera.transform.position.x - halfWidth;
        pos.z = 0f;
        this.Left.position = pos;

	}
}
