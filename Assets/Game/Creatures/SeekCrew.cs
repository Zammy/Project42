using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SeekCrew : AIMovingState
{
    //public GameObject DebugExplosion;

    public float Range;

    Vector2 diffFromCrew;
    Seek seek;

    List<Point> path = new List<Point>();

    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        CalculateDiffFromCrew();
        seek = this.Behaviors.OfType<Seek>().First();

        this.CalculatePathIfNeeded();
    }

    private void CalculateDiffFromCrew()
    {
        this.diffFromCrew = RandomDiffFromCrew();
        //var explosionGo = (GameObject)Instantiate(DebugExplosion, this.CrewPos + diffFromCrew.xToVector3(), Quaternion.identity);
        //explosionGo.transform.parent = Crew.Instance.transform;
    }

    public override void OnExit(AIState nextState)
    {
        base.OnExit(nextState);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (this.path.Count == 0)
            CalculatePathIfNeeded();

        if (this.path.Count > 0)
        {
            seek.Goal = path[0].ToVector2();

            float sqrDisToGoal = (CreatureTransform.transform.position.xToVector2() - path[0].ToVector2()).sqrMagnitude;
            if (sqrDisToGoal < 0.25f)
            {
                path.RemoveAt(0);

                CalculateDiffFromCrew();
            }
        }
        else
        {
            Vector2 crewPos = this.CrewPos.xToVector2();
            Vector2 goal = diffFromCrew + crewPos;

            seek.Goal = goal;

            float sqrDisToGoal = (CreatureTransform.transform.position.xToVector2() - goal).sqrMagnitude;
            if (sqrDisToGoal < 0.25f)
            {
                this.OnCrewSought();
            }
        }
    }

    protected virtual Vector2 RandomDiffFromCrew()
    {
        Vector2 crewPos = this.CrewPos.xToVector2();
        var diffFromCrew = (CreatureTransform.position.xToVector2() - crewPos).normalized * Range;
        return diffFromCrew;
    }

    protected abstract void OnCrewSought();

    private void CalculatePathIfNeeded()
    {
        Vector2 creaturePos = CreatureTransform.position.xToVector2();
        Vector2 crewPos = this.CrewPos.xToVector2();
        Vector2 diff = crewPos - creaturePos;
        RaycastHit2D hit = Physics2D.Raycast(creaturePos, diff, diff.magnitude);
        if (hit.collider != null && hit.collider.tag != "Crew")
        {
            this.path.AddRange(PathFinder.PathFromAtoB(creaturePos.xToPoint(), crewPos.xToPoint()));

            Vector2 prevPoint = creaturePos;
            int pointsLeft = 0;
            do
            {
                for (int i = path.Count - 1; i >= pointsLeft; i--)
                {
                    Vector2 diffPoint = path[i].ToVector2() - prevPoint;
                    RaycastHit2D hitPoint = Physics2D.Raycast(prevPoint, diffPoint, diffPoint.magnitude);
                    if (hitPoint.collider == null)
                    {
                        path.RemoveRange(pointsLeft, i - pointsLeft);
                        break;
                    }
                }

                if (path.Count != pointsLeft)
                    prevPoint = path[pointsLeft++].ToVector2();
            }
            while (path.Count != pointsLeft);
        }
    }
}
