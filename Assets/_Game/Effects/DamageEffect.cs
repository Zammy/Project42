using UnityEngine;
using System.Collections;

public class DamageEffect : MonoBehaviour 
{
    //Set through Unity
    public float Lifetime;
    public Collider Collider;
    //

    float disableAfter;

    void Update()
    {
        if (Time.time > disableAfter)
        {
            Collider.enabled = false;
        }
    }

    public void Activate()
    {
        disableAfter = Time.time + Lifetime;

        transform.xResetParticleSystemsRecursive();

        gameObject.SetActive(true);

        Collider.enabled = true;
    }
}
