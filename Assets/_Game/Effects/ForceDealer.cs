using UnityEngine;
using System.Collections;

public class ForceDealer : MonoBehaviour
{

    public string SourceTag
    {
        get;
        set;
    }

    public float Force { get; set; }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != SourceTag)
        {
            var rigidbody = collider.gameObject.transform.xFindComponentInParents<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddForce( (collider.gameObject.transform.position - this.transform.position).normalized * Force, ForceMode.Impulse);
            }
        }
    }
}