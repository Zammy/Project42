using UnityEngine;
using System.Collections;

public class FollowOnX : MonoBehaviour 
{
    public Transform Follow;

	// Update is called once per frame
	void Update () 
    {
        this.transform.position = new Vector3(Follow.position.x, this.transform.position.y, this.transform.position.z);
	}
}
