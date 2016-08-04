using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageDealer : MonoBehaviour 
{
    public string SourceTag
    {
        get;
        set;
    }

    List<Damage> damages = new List<Damage>();

    public void AddDamage(Damage dmg)
    {
        damages.Add(dmg);
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != SourceTag)
        {
            var charHealth = collider.gameObject.transform.xFindComponentInParents<CharacterHealth>();
            if (charHealth != null)
            {
                foreach (var dmg in damages)
                {
                    charHealth.DealDamage(dmg);
                }
            }
        }
    }
}
