using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour 
{
    //Set through Unity
    public GameObject ExplodePrefab;
    public float Speed;
    public float DistanceLife;
    public bool IsPenetrating = false;
    //

    public bool IsFriendly
    {
        get;
        set;
    }

    public int Damage
    {
        get;
        set;
    }

    List<GameObject> targetsHit = new List<GameObject>();

    void OnDestroy()
    {
        targetsHit.Clear();
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	    float moveBy = this.Speed / (Time.fixedDeltaTime * 1000f);
        this.transform.position += this.transform.up * moveBy;

        this.DistanceLife -= moveBy;

        if (this.DistanceLife <= 0f)
        {
            Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        CharacterHealth charHealth = null;
        if (this.IsFriendly && collider.tag == "Creature")
        {
            charHealth = collider.GetComponent<CharacterHealth>();
        }

        if (!this.IsFriendly && collider.tag == "Player")
        {
            charHealth = collider.GetComponent<CharacterHealth>();
        }


        if (charHealth != null)
        {
            if (charHealth.Health <= 0)
                return;

            if (this.IsPenetrating)
            {
                if (!this.targetsHit.Contains(charHealth.gameObject))
                {
                    targetsHit.Add(charHealth.gameObject);
                    charHealth.DealDamage(this.Damage);
                }
            }
            else
            {
                charHealth.DealDamage(this.Damage);
            }
        }

        if (collider.tag == "Wall" || (this.IsFriendly && collider.tag == "Creature") || (!this.IsFriendly && collider.tag == "Player"))
        {
            if (ExplodePrefab != null)
                Instantiate(ExplodePrefab, this.transform.position, Quaternion.identity);

            if (!this.IsPenetrating)
                Destroy(this.gameObject);
        }
    }
}
