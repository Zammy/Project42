using UnityEngine;
using System.Collections.Generic;

public class Crew : SingletonBehavior<Crew> 
{
    public GameObject CrewMemberPrefab;

    //public GameObject LOSMeshPrefab;

    const float HORZ_CREW_POS = 0.25f;
    const float VERT_CREW_POS = 0.25f;
    CrewMember[] crewMembers;

    public List<CrewMember> CrewMembers
    {
        get
        {
            var crew = new List<CrewMember>(crewMembers.Length);
            foreach (var m in crewMembers) 
            {
                if (m.IsAlive)
                {
                    crew.Add(m);
                }
            }
            return crew;
        }
    }

    public void LoadCrew(CharacterInfo[] characters)
    {
        this.crewMembers = new CrewMember[characters.Length];
        for (int i = 0; i < characters.Length; i++)
        {
            var crewMember = InstantiateCrewMember();
            crewMember.SetCharacterInfo(characters[i]);
            crewMember.Weapon.IsFriendly = true;

            this.crewMembers[i] = crewMember;
        }

        UpdateCrewPositions();
    }

    protected override void OnDestroy()
    {
        foreach (var crew in crewMembers)
        {
            crew.CharHealth.CharacterDied -= OnCrewMemberDied;
        }
    }

    void FixedUpdate()
    {
        if (crewMembers == null) 
            return;

        this.Movement();
        this.LookAt();
        this.FireWeapons();
        this.ActivateSkills();
    }

    CrewMember InstantiateCrewMember()
    {

        var crewMemberGo = (GameObject) Instantiate(CrewMemberPrefab);
        crewMemberGo.transform.SetParent( this.transform );
        crewMemberGo.transform.localScale = Vector3.one;

        //var losMeshGo = (GameObject) Instantiate(LOSMeshPrefab, pos, Quaternion.identity);
        //losMeshGo.transform.SetParent( Camera.main.transform );
//        losMeshGo.transform.localScale = Vector3.one;

        //crewMemberGo.GetComponent<LineOfSightDrawer>().LOS = losMeshGo.transform;

        var member = crewMemberGo.GetComponent<CrewMember>();
        member.CharHealth.CharacterDied += OnCrewMemberDied;
        return member;
    }

    void Movement()
    {
        float lowestSpeed = float.MaxValue;
        foreach (var crewMember in crewMembers)
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
        Vector3 cursorWorldPos = Cursor.CursorPosition;

        this.transform.xLookAt(cursorWorldPos);

        foreach (var crewMember in crewMembers)
        {
            crewMember.PointWeaponAtCursor(cursorWorldPos);
        }
    }

    void FireWeapons()
    {
        var crew = this.CrewMembers;
        bool fire1 = Mathf.Approximately(Input.GetAxis("Fire1"), 1f);
        bool fire2 = Mathf.Approximately(Input.GetAxis("Fire2"), 1f);
        crew[0].Weapon.IsActive = fire1;
        if (crew.Count > 1)
        {
            crew[1].Weapon.IsActive = fire2;
        }
    }

    void OnCrewMemberDied(GameObject character)
    {
        character.SetActive(false);
        if (this.CrewMembers.Count == 0)
        {
            this.gameObject.SetActive(false);
            InGameUI.Instance.CrewKilled();
        }
        UpdateCrewPositions();
    }

    void UpdateCrewPositions()
    {
        var crew = this.CrewMembers;
        int crewCount = crew.Count;

        Vector3 worldPos = this.transform.position;
        switch (crewCount)
        {
            case 1:
            {
                crew[0].transform.position = worldPos;
                break;
            }
            case 2:
            {
                
                crew[0].transform.position = worldPos + new Vector3(-HORZ_CREW_POS, 0);
                crew[1].transform.position = worldPos + new Vector3(+HORZ_CREW_POS, 0);
                break;
            }
            case 3:
            {
                crew[0].transform.position = worldPos + new Vector3(-HORZ_CREW_POS, VERT_CREW_POS);
                crew[1].transform.position = worldPos + new Vector3(+HORZ_CREW_POS, VERT_CREW_POS);
                crew[2].transform.position = worldPos + new Vector3(0, -VERT_CREW_POS);
                break;
            }
            case 4:
            {
                crew[0].transform.position = worldPos + new Vector3(-HORZ_CREW_POS, VERT_CREW_POS);
                crew[1].transform.position = worldPos + new Vector3(+HORZ_CREW_POS, VERT_CREW_POS);
                crew[2].transform.position = worldPos + new Vector3(-HORZ_CREW_POS, -VERT_CREW_POS);
                crew[3].transform.position = worldPos + new Vector3(+HORZ_CREW_POS, -VERT_CREW_POS);
                break;
            }
            default:
                break;
        }

    }

    void ActivateSkills()
    {
        var crewMembers = this.CrewMembers;
        //int keyCode = 49; //Alpha1
        for (int i = 0, keyCode = 49; i < crewMembers.Count; i++, keyCode++)
        {
            if (Input.GetKeyDown((KeyCode)keyCode))
            {
                crewMembers[i].ActivateSkill();
            }
        }
    }

    //    void LateUpdate()
    //    {
    //        this.GenLineOfSight();
    //    }

    //void GenLineOfSight()
    //{
    //    var pois = this.Level.GetPOIS();
    //    foreach (var member in crew)
    //    {
    //        var losDrawer = member.GetComponent<LineOfSightDrawer>();

    //        losDrawer.GenerateLOSMesh(pois);
    //    }
    //}
}
