using UnityEngine;
using System.Collections;

public abstract class ZoneCommand : MonoBehaviour
{
    public abstract IEnumerator Execute();
}
