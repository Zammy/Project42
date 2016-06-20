using UnityEngine;
using System.Collections;

public class CharacterInput : MonoBehaviour 
{
    PlayerActions playerActions;
    CharacterMovement charMove;

	void Start () 
    {
        charMove = GetComponent<CharacterMovement>();
	    playerActions = PlayerActions.CreateWithDefaultBindings();
	}
	
	// Update is called once per frame
	void Update () 
    {
        var moveVec = playerActions.Move.Value;
        charMove.MovementDirection = new Vector3( -moveVec.x, 0, -moveVec.y);
	}
}
