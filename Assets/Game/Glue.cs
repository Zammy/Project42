using UnityEngine;
using System.Collections;

public class Glue : MonoBehaviour 
{
    //Set through Unity
    public Crew Crew;
    public LevelExt Level;
    //

	// Use this for initialization
	void Start () 
    {
        this.Crew.LoadCrew( new CrewType[] { CrewType.Assault/*, CrewType.Marine , CrewType.Medic */} );
	}
	

}
