using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HitEffect : MonoBehaviour 
{
    public MeshRenderer Renderer;

    float dieAfter;

    public float Life
    {
        get;
        set;
    }

    void Start()
    {
        dieAfter = Time.time + Life;
    }

	// Update is called once per frame
	void Update () 
    {
	    if (Time.time > dieAfter)
        {
            Destroy(this.gameObject);
        }
    }

//    void SetTextureOffset()
//    {
//        foreach (var renderer in AnimateMaterials)
//        {
//            renderer.material.SetTextureOffset("_MainTex", new Vector2(x, 0));
//        }
//    }
}
