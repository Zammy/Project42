using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
    //Set through Unity
    public GameObject ExplodePrefab;
    public float Speed;
    public float DistanceLife;
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
        if (collider.tag == "Wall")
        {
            if (ExplodePrefab != null)
            {
                //explode
                Instantiate(ExplodePrefab, this.transform.position, Quaternion.identity);
            }
            

            //TODO: deal damage

            Destroy(this.gameObject);
        }
    }
}
