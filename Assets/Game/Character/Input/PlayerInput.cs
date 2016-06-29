using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInput : MonoBehaviour 
{
    PlayerActions playerActions;
    CharacterSkills charSkills;
    CharacterMovement charMove;

    PlayerAction[] skills;

	void Start () 
    {
        charMove = GetComponent<CharacterMovement>();
        charSkills = GetComponent<CharacterSkills>();

        playerActions = PlayerActions.CreateWithDefaultBindings();

        skills = new PlayerAction[] { playerActions.Skill1, playerActions.Skill2, playerActions.Skill3, playerActions.Skill4 };
	}
	
	// Update is called once per frame
	void Update () 
    {
        var moveVec = playerActions.Move.Value;
        charMove.MovementDirection = new Vector3( -moveVec.x, 0, -moveVec.y);

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].WasPressed)
            {
                charSkills.ExecuteSkill(i);
            }
        }
	}
}
