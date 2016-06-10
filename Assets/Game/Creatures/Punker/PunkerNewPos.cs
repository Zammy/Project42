using UnityEngine;
using System.Linq;

public class PunkerNewPos : AIMovingState
{
    public float AroundAngle;
    public float AroundDistance;

    public GameObject DebugExplosion;

    Seek seek;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        seek = this.Behaviors.OfType<Seek>().First();

        Vector3 creaturePos = CreatureTransform.position;
        Vector3 toCrew = (this.CrewPos - creaturePos).normalized;

        RaycastHit2D hit;
        Vector3 goal = Vector3.zero;
        float aroundAngle = AroundAngle;
        for (int i = 0; i < 25; i++)
        {
            float angle = 90f;
            bool left = UnityEngine.Random.Range(0, 2) == 0;
            if (left)
            {
                angle = -angle;
            }

            var rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(angle - aroundAngle, angle + aroundAngle));
            Vector3 dir = rotation * toCrew;

            float distance = UnityEngine.Random.Range(1f, AroundDistance);
            hit = Physics2D.Raycast(creaturePos.xToVector2(), dir.xToVector2(), distance);
            if (hit.collider == null)
            { 
                goal = creaturePos + (dir * distance);
                break;
            }

            aroundAngle += 1f;
        }
 
        seek.Goal = goal;

        var explosionGo = (GameObject)Instantiate(DebugExplosion, seek.Goal, Quaternion.identity);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        float sqrDisToGoal = (this.CrewPos - CreatureTransform.position).sqrMagnitude;
        if (sqrDisToGoal < 0.25f)
        {
            StateManager.ActivateState<PunkerAttack>();
        }
    }

    protected override void OnNotMoving()
    {
        StateManager.ActivateState<PunkerAttack>();
    }
}
