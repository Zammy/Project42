using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    //Set through Unity
    public float Speed;
    public GameObject ExplosionPrefab;
    //

    public string SourceTag { get; set; }

    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != SourceTag)
        {
            Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
            StartCoroutine(DestroyOnNextFrame());
        }
    }

    void FixedUpdate()
    {
        body.velocity = this.transform.forward * Speed;
    }

    IEnumerator DestroyOnNextFrame()
    {
        yield return null;

        Destroy(this.gameObject);
    }
}
