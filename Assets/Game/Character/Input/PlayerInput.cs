using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInput : MonoBehaviour 
{
    public Skill[] Skills;

    PlayerActions playerActions;
    CharacterMovement charMove;
    PlayerAction[] skillsMapping;

	void Start () 
    {
        charMove = GetComponent<CharacterMovement>();

        playerActions = PlayerActions.CreateWithDefaultBindings();

        skillsMapping = new PlayerAction[] { playerActions.Skill1, playerActions.Skill2, playerActions.Skill3, playerActions.Skill4 };
	}
	
	void Update () 
    {
        var moveVec = playerActions.Move.Value;
        charMove.MovementDirection = new Vector3( -moveVec.x, 0, -moveVec.y);

        for (int i = 0; i < Skills.Length; i++)
        {
            if (skillsMapping[i].IsPressed)
            {
                Skills[i].Activate();
            }
            else
            {
                Skills[i].Deactivate();
            }
        }
	}
}
