using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharactersManager : SingletonBehavior<CharactersManager> 
{
    List<GameObject> enemies = new List<GameObject>();

    public GameObject Player { get; set; }

	protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            var character = transform.GetChild(i);

            if (character.tag == "Player")
            {
                Player = character.gameObject;
            }
            else if (character.tag == "Enemy")
            {
                enemies.Add(character.gameObject);
            }
            else
            {
                throw new UnityException("Found an object in Characters without a tag!");
            }
        }
    }

    public void AddCharacter(GameObject enemyGo)
    {
        enemies.Add(enemyGo);
        enemyGo.transform.SetParent(this.transform);
    }
}
