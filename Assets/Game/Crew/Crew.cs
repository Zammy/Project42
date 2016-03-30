using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crew : MonoBehaviour 
{
    public GameObject CrewMemberPrefab;
    public float TempSpeed;

    const float HORZ_CREW_POS = 0.557f;
    const float VERT_CREW_POS = 0.318f;

    CrewMember[] crew;

    public void LoadCrew(CrewType[] crewTypes)
    {
        var crewPoses = new List<Vector3>();
        if (crewTypes.Length == 1)
        {
            crewPoses.Add(Vector3.zero);
        }
        else if (crewTypes.Length == 2)
        {
            crewPoses.Add(new Vector3(-HORZ_CREW_POS, 0));
            crewPoses.Add(new Vector3(+HORZ_CREW_POS, 0));
        }
        else if (crewTypes.Length == 3)
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

        this.crew = new CrewMember[crewTypes.Length];

        for (int i = 0; i < crewTypes.Length; i++)
        {
            Vector3 pos = crewPoses[i];
            CrewType type = crewTypes[i] ;
            var crewMember = InstantiateCrewMember(pos);
            crewMember.SetCrewType(type);
            this.crew[i] = crewMember;
        }
       
    }

    CrewMember InstantiateCrewMember(Vector3 localPos)
    {
        Vector3 pos = this.transform.position + localPos ;

        var crewMemberGo = (GameObject) Instantiate(CrewMemberPrefab, pos, Quaternion.identity);
        crewMemberGo.transform.SetParent( this.transform );
        crewMemberGo.transform.localScale = Vector3.one;

        return crewMemberGo.GetComponent<CrewMember>();
    }

    void Movement()
    {
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
        Vector3 move = new Vector3();
        if (!Mathf.Approximately(horz, 0f))
        {
            move.x = horz;
        }
        if (!Mathf.Approximately(vert, 0f))
        {
            move.y = vert;
        }
        move.Normalize();
        move = move * (Time.fixedDeltaTime * TempSpeed);
        this.transform.position += move;
    }        

    void FixedUpdate()
    {
        this.Movement();

        bool fire1 = Mathf.Approximately(Input.GetAxis("Fire1"), 1f);
        bool fire2 = Mathf.Approximately(Input.GetAxis("Fire2"), 1f);

        this.crew[0].Weapon.IsActive = fire1;
        if (this.crew.Length > 1)
        {
            this.crew[1].Weapon.IsActive = fire2;
        }
    }
}
