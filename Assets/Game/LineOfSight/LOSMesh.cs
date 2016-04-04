using UnityEngine;
using System.Collections;

public class LOSMesh : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        var meshFilter = this.GetComponent<MeshFilter>();
        var m = new Mesh();
        meshFilter.mesh = m;
	}
}
