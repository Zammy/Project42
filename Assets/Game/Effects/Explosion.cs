using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public float Life;

	void Start ()
    {
        StartCoroutine(Die());
	}

    IEnumerator Die()
    {
        yield return new WaitForSeconds(Life);

        Destroy(this.gameObject);
    }
}
