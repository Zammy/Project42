using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour 
{
    public MeshRenderer[] AnimateMaterials;

    public float Life
    {
        get;
        set;
    }

    float x = -1f;
    float animSpeed;

    void Start()
    {
        SetTextureOffset();

        animSpeed =  2f / Life;
    }

	// Update is called once per frame
	void Update () 
    {
	    x += animSpeed * Time.deltaTime;

        if (x > 1f) 
        {
            Destroy(this.gameObject);
        }
        SetTextureOffset();
	}

    void SetTextureOffset()
    {
        foreach (var renderer in AnimateMaterials)
        {
            renderer.material.SetTextureOffset("_MainTex", new Vector2(x, 0));
        }
    }
}
