using System;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public event Action Trigger;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            OnRaiseTrigger();
        }
    }

    void OnRaiseTrigger()
    {
        if (Trigger != null)
        {
            Trigger();
        }
    }
}
