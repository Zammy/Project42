using UnityEngine;
using System.Collections;

public class AccountForRootMotion : MonoBehaviour
{
    public Transform TargetTrans;

    void OnAnimatorMove()
    {
        var localPos = transform.position;
        Animator animator = GetComponent<Animator>();
        Vector3 delta = animator.deltaPosition;
        localPos.y += delta.y;
        //delta.y = 0f;
        TargetTrans.position += delta;
        //transform.position = localPos;
    }
}
