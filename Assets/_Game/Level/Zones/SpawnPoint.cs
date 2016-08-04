using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    public GameObject EnemyPrefab;

    CharactersManager charMnger;

    void Awake()
    {
        charMnger = CharactersManager.Instance;
    }

    public void Spawn()
    {
        GameObject enemyGo = (GameObject)Instantiate(EnemyPrefab, this.transform.position, Quaternion.identity);
        charMnger.AddCharacter(enemyGo);
    }
}
