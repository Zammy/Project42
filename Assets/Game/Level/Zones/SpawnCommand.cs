using UnityEngine;
using System.Collections;
using System;

public class SpawnCommand : ZoneCommand
{

    public SpawnPoint[] SpawnPoints;

    public override IEnumerator Execute()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnPoints[i].Spawn();
        }

        yield return null;
    }
}
