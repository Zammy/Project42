using UnityEngine;
using System.Collections;
using System;

public class MoveCharacterCommand : ZoneCommand
{
    public Transform[] Waypoints;
    public CharacterMovement CharMovement;

    public override IEnumerator Execute()
    {
        int currentWaypoint = 0;
        do
        {
            Vector3 waypoint = Waypoints[currentWaypoint].position;
            Vector3 diff = waypoint - CharMovement.transform.position;

            CharMovement.MovementDirection = diff.normalized;

            if (diff.sqrMagnitude < 0.5f)
            {
                currentWaypoint++;
            }
            yield return null;
        }
        while (currentWaypoint < Waypoints.Length);
    }
}
