using UnityEngine;
using System.Collections;

public class Glue : MonoBehaviour 
{
    //Set through Unity
    public Crew Crew;
    public LevelExt Level;
    public CellularAutomata CelAut;

    public Transform PremadeLevel;
    //

	// Use this for initialization
	void Awake () 
    {
        if (PremadeLevel.gameObject.activeSelf && PremadeLevel.childCount > 0)
        {
            this.Level.GetTilesFrom(PremadeLevel);
        }
        else
        {
            this.CelAut.GenerateDungeon();
        }

        this.Crew.LoadCrew( new CrewType[] { CrewType.Assault /*, CrewType.Marine , CrewType.Medic*/ } );
    }
	

}
