using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMember : MonoBehaviour 
{
    public CharacterInfo CharInfo 
    {
        get;
        private set;
    }

    public Weapon Weapon
    {
        get;
        set;
    }

    public void SetCharacterInfo(CharacterInfo charInfo)
    {
        this.CharInfo = GameObject.Instantiate(charInfo) as CharacterInfo;

        GameObject weaponPrefab = this.CharInfo.Weapon;

        var weaponGo = (GameObject) Instantiate(weaponPrefab, this.transform.position, Quaternion.identity);
        weaponGo.transform.SetParent(this.transform);
        weaponGo.transform.localScale = Vector3.one;

        this.Weapon = weaponGo.GetComponent<Weapon>();

        this.GetComponent<CharacterHealth>().Health = charInfo.HP;
    }

    public void PointWeaponAtCursor(Vector3 cursorPos)
    {
        this.Weapon.transform.xLookAt(cursorPos);
    }
}
