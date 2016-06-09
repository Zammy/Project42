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
        Vector3 toCrew = (Crew.Instance.transform.position - creaturePos).normalized;

        float angle = 90f;
        bool left = UnityEngine.Random.Range(0, 2) == 0;
        if (left)
        {
            angle = -angle;
        }

        var rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(angle - AroundAngle, angle + AroundAngle));
        Vector3 dir = rotation * toCrew;

        RaycastHit2D hit = Physics2D.Raycast(creaturePos.xToVector2(), dir.xToVector2(), AroundDistance);
        if (hit.collider != null)
        {
            seek.Goal = hit.point.xToVector3();
        }
        else
        {
            seek.Goal = creaturePos + (dir * AroundDistance);
        }

        var explosionGo = (GameObject)Instantiate(DebugExplosion, seek.Goal, Quaternion.identity);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        float sqrDisToGoal = (Crew.Instance.transform.position - CreatureTransform.position).sqrMagnitude;
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
