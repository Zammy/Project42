using UnityEngine;
using System.Collections;

public class Glue : MonoBehaviour 
{
    //Set through Unity
    public Crew Crew;
    public CharacterInfo[] Characters;
    //

	void Awake () 
    {
        this.Crew.LoadCrew(  Characters );
    }
}
