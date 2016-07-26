using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void xAddUpTo<T> (this List<T> list, int count)
    {
        for (int i = list.Count; i <= count; i++)
        {
            list.Add(default(T));
        }
    }

    public static IEnumerator xWaitForState(this Animator animator, string stateName)
    {
        while (true)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName(stateName))
            {
                break;
            }
            yield return null;
        }
    }

    public static IEnumerator xWaitWhileState(this Animator animator, string stateName)
    {
        while (true)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName(stateName))
            {
                break;
            }
            yield return null;
        }
    }
}

