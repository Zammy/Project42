using UnityEngine;
using System.Collections;

public class Perishable : MonoBehaviour 
{
	void Update () 
    {
	    AnimatorStateInfo animatorState = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (animatorState.IsName("Done"))
        {
            Destroy(this.gameObject);
        }
	}
}
