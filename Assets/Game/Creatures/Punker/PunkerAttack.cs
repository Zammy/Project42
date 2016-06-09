
public class PunkerAttack : Attack
{
    public override void OnEnter(AIState previousState)
    {
        base.OnEnter(previousState);

        CreatureTransform.transform.xLookAt(Crew.Instance.transform.position);
    }

    protected override void OnAttackFinished()
    {
        StateManager.ActivateState<PunkerHideBehindObstacle>();
    }
}
