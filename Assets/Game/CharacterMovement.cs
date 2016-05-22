using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
    //Set through Unity
    public float Speed;
    //

    public Vector2 MovementDirection
    {
        get;
        set;
    }

    protected virtual void FixedUpdate()
    {
        if (MovementDirection.sqrMagnitude < 0.5f)
            return;

        var move = new Vector3(this.MovementDirection.x, this.MovementDirection.y) * (this.Speed * Time.fixedDeltaTime);
        transform.position += move;
    }
}
