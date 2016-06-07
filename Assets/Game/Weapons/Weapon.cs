using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
    //Set through Unity
    public GameObject ProjPrefab;
    public Transform Nuzzle;

    public int Damage;
    public float RateOfFire;
    public float Accuracy; //lower is better
    public bool IsFriendly;
    //

    private bool isActive = false;
    public virtual bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            if (value == isActive) 
                return;
            this.isActive = value;
        }
    }

    private float shootTime;

    protected virtual void Update()
    {
        if (this.IsActive)
        {
            if (Time.time > this.shootTime)
            {
                this.Shoot();
            }
        }
    }

    protected virtual void Shoot()
    {
        this.ShootProj();

        shootTime = Time.time + RateOfFire;
    }

    protected virtual void ShootProj()
    {
        //todo spawn and shoot projectile
        var projGo = (GameObject)Instantiate(ProjPrefab, this.Nuzzle.position, this.Nuzzle.rotation);
        var proj = projGo.GetComponent<Projectile>();
        proj.Damage = this.Damage;
        proj.IsFriendly = this.IsFriendly;

        proj.transform.Rotate(Vector3.forward, Random.Range(-Accuracy, Accuracy));

    }
}
