using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMember : MonoBehaviour 
{
    //Set through Unity
    public GameObject PistolPrefab;
    public GameObject AssaultRilfePrefab;
    public GameObject ShotgunPrefab;
    //

    Dictionary<CrewType, GameObject> crewTypeToWeapPrefab;

    public Weapon Weapon
    {
        get;
        set;
    }

    void Awake()
    {
        crewTypeToWeapPrefab = new Dictionary<CrewType, GameObject>()
        {
            { CrewType.Marine, AssaultRilfePrefab },
            { CrewType.Assault, ShotgunPrefab},
            { CrewType.Medic, PistolPrefab }
        };
    }

	public void SetCrewType(CrewType type)
    {
        GameObject weaponPrefab = crewTypeToWeapPrefab[type];

        var weaponGo = (GameObject) Instantiate(weaponPrefab, this.transform.position, Quaternion.identity);
        weaponGo.transform.SetParent(this.transform);
        weaponGo.transform.localScale = Vector3.one;

        this.Weapon = weaponGo.GetComponent<Weapon>();
    }

    public void PointWeaponAtCursor(Vector3 cursorPos)
    {
        this.Weapon.transform.xLookAt(cursorPos);
    }
}
