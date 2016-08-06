using UnityEngine;
using System.Collections;

public class PlayerHealthWatcher : MonoBehaviour 
{

    CharacterHealth charHealth;

	// Use this for initialization
	void Start () 
    {
	    charHealth = GetComponent<CharacterHealth>();
        InGameUI.Instance.SetPlayerHealth(charHealth.Health);
        charHealth.DealtDamage += this.OnDamageDelt;
	}

    void OnDestroy()
    {
        charHealth.DealtDamage -= this.OnDamageDelt;
    }
	
    void OnDamageDelt(Damage dmg)
    {
        InGameUI.Instance.SetPlayerHealth(charHealth.Health);
    }
}
