using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
    //Set through Unity
    public float Speed;
    //

    public Vector2 Direction
    {
        get;
        set;
    }

    void FixedUpdate()
    {
        if (Direction.sqrMagnitude < 0.5f)
            return;

        var move = new Vector3(this.Direction.x, this.Direction.y) * (this.Speed * Time.fixedDeltaTime);
        this.transform.position += move;
    }
}
