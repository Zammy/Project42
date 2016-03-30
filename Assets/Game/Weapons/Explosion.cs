using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
    {
	    AnimatorStateInfo animatorState = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (animatorState.IsName("Done"))
        {
            Destroy(this.gameObject);
        }
	}
}
