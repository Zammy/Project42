using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureMovement : CharacterMovement 
{
    public Transform BehaviorsBase;

    AIBehavior[] behaviors;

    List<Vector3[]> interestMaps = new List<Vector3[]>();
    List<Vector3[]> dangerMaps = new List<Vector3[]>();

	protected override void Start () 
    {
        base.Start();

        this.behaviors = BehaviorsBase.GetComponentsInChildren<AIBehavior>();
    }

    protected override void Update()
    {
        this.interestMaps.Clear();
        this.dangerMaps.Clear();

        foreach (var beh in behaviors)
        {
            if (beh.isActiveAndEnabled)
            {
                var interest = beh.GetInterest();
                interestMaps.Add(interest);

                var danger = beh.GetDanger();
                dangerMaps.Add(danger);
            }
        }

        var newDirection = Vector3.zero;
        for (int i = 0; i < interestMaps.Count; i++)
        {
            var interestMap = interestMaps[i];
            for (int y = 0; y < interestMap.Length; y++)
            {
                newDirection += interestMap[y];
            }
        }

        if (newDirection != Vector3.zero)
        {
            for (int i = 0; i < dangerMaps.Count; i++)
            {
                var dangerMap = dangerMaps[i];
                for (int y = 0; y < dangerMap.Length; y++)
                {
                    newDirection += dangerMap[y];
                }
            }
        }

        if (newDirection.sqrMagnitude > 0.15f)
        {
            newDirection.Normalize();

            this.MovementDirection = newDirection;
        }
        else
        {
            this.MovementDirection = Vector3.zero;
        }

        base.Update();
    }
}
