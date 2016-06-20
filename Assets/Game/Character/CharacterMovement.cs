using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
    //Set through Unity
    public float Speed;
    //

    public Vector3 MovementDirection
    {
        get;
        set;
    }

    protected virtual void Update()
    {
        if (MovementDirection.sqrMagnitude < 0.5f)
            return;

        var move = MovementDirection * (this.Speed * Time.deltaTime);
        transform.position += move;

        transform.localRotation = Quaternion.LookRotation(MovementDirection);
    }
}
