using UnityEngine;
using System.Collections;

public class Glue : MonoBehaviour 
{
    //Set through Unity
    public Crew Crew;
    //

	// Use this for initialization
	void Start () 
    {
	    this.Crew.LoadCrew( new CrewType[] { CrewType.Assualt, CrewType.Marine, CrewType.Medic } );
	}
	

}
