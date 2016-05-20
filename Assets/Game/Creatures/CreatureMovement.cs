using UnityEngine;
using System.Collections;

public class CreatureMovement : CharacterMovement 
{
    AIBehavior[] behaviors;

    
	void Start () 
    {
        this.behaviors = this.GetComponents<AIBehavior>();
	}

    protected override void FixedUpdate()
    {
        Vector2 dir = Vector2.zero;
        foreach (var beh in behaviors)
        {
            if (beh.isActiveAndEnabled)
            {
                dir += beh.CalculateDirection();
            }
        }

        if (dir.sqrMagnitude > 0.05f)
        {
            this.transform.xLookAt(this.transform.position + new Vector3(dir.x, dir.y));

            dir.Normalize();
            this.Direction = dir;
        }
        else
        {
            this.Direction = Vector2.zero;
        }

        base.FixedUpdate();
    }
}
