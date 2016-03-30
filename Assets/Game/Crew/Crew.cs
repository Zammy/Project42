using UnityEngine;
using System.Collections;

public class Crew : MonoBehaviour 
{
    public GameObject CrewPrefab;
    public float TempSpeed;

    const float HORZ_CREW_POS = 0.557f;
    const float VERT_CREW_POS = 0.318f;


    void FixedUpdate()
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

        bool fire1 = Mathf.Approximately(Input.GetAxis("Fire1"), 1f);
        bool fire2 = Mathf.Approximately(Input.GetAxis("Fire2"), 1f);

        Vector3 move = new Vector3();
        if (!Mathf.Approximately(horz, 0f) )
        {
            move.x = horz;
        }

        if (!Mathf.Approximately(vert, 0f) )
        {
            move.y = vert;
        }
        move.Normalize();
        move = move * (Time.fixedDeltaTime * TempSpeed);

        this.transform.position += move;
    }


    public void LoadCrew(CrewType[] crew)
    {
        
    }
}
