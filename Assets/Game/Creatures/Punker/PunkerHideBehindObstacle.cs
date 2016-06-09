using UnityEngine;
using System.Linq;

public class PunkerHideBehindObstacle : AIMovingState
{
    public float WaitBehindCover;
    public float CoverSearchDistance;

    public GameObject DebugExplosion;

    Seek seek;
    float waitUntil;

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        waitUntil = float.MinValue;

        seek = this.Behaviors.OfType<Seek>().First();

        var obstacles = Level.Instance.GetObstaclesAround(CreatureTransform.position, CoverSearchDistance);
        if (obstacles.Count > 0)
        {
            var obstacle = obstacles[UnityEngine.Random.Range(0, obstacles.Count)];

            Vector3 toCrew = Crew.Instance.transform.position - obstacle.transform.position;
            var behindObstacle = toCrew.normalized * 1.5f;

            seek.Goal = obstacle.transform.position - behindObstacle;

            Instantiate(DebugExplosion, seek.Goal, Quaternion.identity);
        }
        else
        {
            this.StateManager.ActivateState<PunkerNewPos>();
        }
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (seek.Goal != Vector3.zero)
        {
            return;
        }

        if (waitUntil < 0f)
        {
            waitUntil = Time.time + this.WaitBehindCover;

        }
            
        if (Time.time > waitUntil)
        {
            StateManager.ActivateState<PunkerNewPos>();
        }
    }

    protected override void OnNotMoving()
    {
        StateManager.ActivateState<PunkerNewPos>();
    }

}
