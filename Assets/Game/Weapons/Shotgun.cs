using UnityEngine;
using System.Collections;

public class Shotgun : Weapon 
{
    //Set through Unity
    public int NumProjs;
    //

    protected override void Shoot()
    {
        base.Shoot();

        for (int i = 0; i < NumProjs-1; i++)
        {
            this.ShootProj();
        }
    }
}
