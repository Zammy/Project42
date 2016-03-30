using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
    //Set through Unity
    public GameObject ExplodePrefab;
    public float Speed;
    //

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
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Crew")
        {
            return;
        }

        //explode
        Instantiate(ExplodePrefab, this.transform.position, Quaternion.identity);

        //TODO: deal damage

        Destroy(this.gameObject);
    }
}
