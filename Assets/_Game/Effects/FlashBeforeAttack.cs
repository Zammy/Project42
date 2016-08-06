using UnityEngine;
using System.Collections;

public class FlashBeforeAttack : CharFlash
{
    public void BeforeAttack()
    {
        StartCoroutine( Flash());
    }
}
