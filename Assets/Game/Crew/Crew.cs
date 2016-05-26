using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crew : SingletonBehavior<Crew> 
{
    public GameObject CrewMemberPrefab;

    public GameObject LOSMeshPrefab;
    public LevelExt Level;

    const float HORZ_CREW_POS = 0.25f;
    const float VERT_CREW_POS = 0.25f;
    CrewMember[] crew;

    public void LoadCrew(CharacterInfo[] characters)
    {
        var crewPoses = new List<Vector3>();
        if (characters.Length == 1)
        {
            crewPoses.Add(Vector3.zero);
        }
        else if (characters.Length == 2)
        {
            crewPoses.Add(new Vector3(-HORZ_CREW_POS, 0));
            crewPoses.Add(new Vector3(+HORZ_CREW_POS, 0));
        }
        else if (characters.Length == 3)
        {
            crewPoses.Add(new Vector3(-HORZ_CREW_POS, VERT_CREW_POS));
            crewPoses.Add(new Vector3(+HORZ_CREW_POS, VERT_CREW_POS));
            crewPoses.Add(new Vector3(0, -VERT_CREW_POS));
        }
        else
        {
            crewPoses.Add(new Vector3(-HORZ_CREW_POS, VERT_CREW_POS));
            crewPoses.Add(new Vector3(+HORZ_CREW_POS, VERT_CREW_POS));
            crewPoses.Add(new Vector3(-HORZ_CREW_POS, -VERT_CREW_POS));
            crewPoses.Add(new Vector3(+HORZ_CREW_POS, -VERT_CREW_POS));
        }

        this.crew = new CrewMember[characters.Length];
        for (int i = 0; i < characters.Length; i++)
        {
            Vector3 pos = crewPoses[i];
            var crewMember = InstantiateCrewMember(pos);
            crewMember.SetCharacterInfo(characters[i]);
            crewMember.Weapon.IsFriendly = true;

            this.crew[i] = crewMember;
        }

    }

    CrewMember InstantiateCrewMember(Vector3 localPos)
    {
        Vector3 pos = this.transform.position + localPos ;

        var crewMemberGo = (GameObject) Instantiate(CrewMemberPrefab, pos, Quaternion.identity);
        crewMemberGo.transform.SetParent( this.transform );
        crewMemberGo.transform.localScale = Vector3.one;

        var losMeshGo = (GameObject) Instantiate(LOSMeshPrefab, pos, Quaternion.identity);
        losMeshGo.transform.SetParent( Camera.main.transform );
//        losMeshGo.transform.localScale = Vector3.one;

        crewMemberGo.GetComponent<LineOfSightDrawer>().LOS = losMeshGo.transform;

        var member = crewMemberGo.GetComponent<CrewMember>();
        return member;
    }

    void Movement()
    {
        float lowestSpeed = float.MaxValue;
        foreach (var crewMember in crew)
        {
            float speed = crewMember.CharInfo.Speed;
            if (crewMember.CharInfo.Speed < lowestSpeed)
            {
                lowestSpeed = speed;
            }
        }
        this.GetComponent<CharacterMovement>().Speed = lowestSpeed;


        float horz = 0f;
        float vert = 0f;
        //        float horz = Input.GetAxis("Horizontal");
        //        float vert = Input.GetAxis("Vertical");
        //        if (horz > 0) horz = 1f; else if (horz < 0) horz = -1f;
        //        if (vert > 0) vert = 1f; else if (vert < 0) vert= -1f;
        if (Input.GetKey(KeyCode.A))
        {
            horz = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horz = +1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            vert = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vert = -1f;
        }
        Vector3 dir = new Vector3();
        if (!Mathf.Approximately(horz, 0f))
        {
            dir.x = horz;
        }
        if (!Mathf.Approximately(vert, 0f))
        {
            dir.y = vert;
        }
        dir.Normalize();

        this.GetComponent<CharacterMovement>().MovementDirection = dir;
    }

    void LookAt()
    {
        Vector3 cursorScreenPos = Input.mousePosition;
        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(cursorScreenPos);
        cursorWorldPos.z = 0;

        this.transform.xLookAt(cursorWorldPos);

        foreach (var crewMember in crew)
        {
            crewMember.PointWeaponAtCursor(cursorWorldPos);
        }
    }

    void FireWeapons()
    {
        bool fire1 = Mathf.Approximately(Input.GetAxis("Fire1"), 1f);
        bool fire2 = Mathf.Approximately(Input.GetAxis("Fire2"), 1f);
        this.crew[0].Weapon.IsActive = fire1;
        if (this.crew.Length > 1)
        {
            this.crew[1].Weapon.IsActive = fire2;
        }
    }

    void FixedUpdate()
    {
        if (crew == null) return;

        this.Movement();
        this.LookAt();
        this.FireWeapons();
    }

    void LateUpdate()
    {
//        this.GenLineOfSight();
    }

    void GenLineOfSight()
    {
        var pois = this.Level.GetPOIS();
        foreach (var member in crew)
        {
            var losDrawer = member.GetComponent<LineOfSightDrawer>();

            losDrawer.GenerateLOSMesh(pois);
        }
    }
}
