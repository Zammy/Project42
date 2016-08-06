using UnityEngine;
using System.Collections;

public class CharFlash : MonoBehaviour 
{
    //Set through Unity
    public Renderer Renderer;
    public Material FlashMaterial;
    //

    Material material;

    protected virtual void Start()
    {
        material = Renderer.material;
    }

    protected IEnumerator Flash()
    {
        for (int i = 0; i < 3; i++)
        {
            Renderer.material = FlashMaterial;

            yield return null;
            yield return null;

            Renderer.material = material;

            yield return null;
            yield return null;
        }
    }
}
