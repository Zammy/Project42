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
            { CrewType.Assualt, ShotgunPrefab},
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

    public void LookAtCursor(Vector3 cursorPos)
    {
        this.LookAt(cursorPos);

        Vector3 fromPosToPoint = cursorPos - this.transform.position;
        float angle = Mathf.Acos( Weapon.NuzzleOffset / fromPosToPoint.magnitude );

        Vector3 cursorOffset = Quaternion.AngleAxis(-angle * Mathf.Rad2Deg, Vector3.forward) * fromPosToPoint;

        cursorOffset = cursorOffset.normalized * Weapon.NuzzleOffset;

        Vector3 actualPoint = cursorPos - cursorOffset;

        this.LookAt(actualPoint);
    }

    public void LookAt(Vector3 point)
    {
        Vector3 fromPosToPoint = point - this.transform.position;
        Vector3 dir = fromPosToPoint.normalized;
        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
