using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugPoint : MonoBehaviour {

    public Text Text;

	public int Num 
    {
        set
        {
            this.Text.text = value.ToString();
        }

    }
}
