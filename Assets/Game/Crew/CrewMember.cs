using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMember : MonoBehaviour
{
    public GameObject BlinkIn;
    public GameObject BlinkOut;

    public CharacterInfo CharInfo { get; private set; }
    public Weapon Weapon { get; private set; }

    public CharacterHealth CharHealth
    {
        get
        {
            return this.GetComponent<CharacterHealth>();
        }
    }

    public bool IsAlive
    {
        get
        {
            return CharHealth.Health > 0;
        }
    }

    Animator animator;
    float skillRechargesUntil = 0f;

    public void SetCharacterInfo(CharacterInfo charInfo)
    {
        this.CharInfo = GameObject.Instantiate(charInfo) as CharacterInfo;

        GameObject weaponPrefab = this.CharInfo.Weapon;

        var weaponGo = (GameObject)Instantiate(weaponPrefab, this.transform.position, Quaternion.identity);
        weaponGo.transform.SetParent(this.transform);
        weaponGo.transform.localScale = Vector3.one;

        this.Weapon = weaponGo.GetComponent<Weapon>();

        this.CharHealth.Health = charInfo.HP;
    }

    public void PointWeaponAtCursor(Vector3 cursorPos)
    {
        this.Weapon.transform.xLookAt(cursorPos);
    }

    public void ActivateSkill()
    {
        if (Time.time < skillRechargesUntil)
            return;

        skillRechargesUntil = Time.time + CharInfo.ActiveSkill.Recharges;

        switch (CharInfo.ActiveSkill.Type)
        {
            case ActiveSkillType.Blink:
                this.Blink();
                break;
            case ActiveSkillType.AOESlowdown:
                break;
            case ActiveSkillType.Shield:
                break;
            case ActiveSkillType.FakeTarget:
                break;
            default:
                break;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Recharging", Time.time < skillRechargesUntil);
    }

    void Blink()
    {
        Instantiate(BlinkIn, transform.position, Quaternion.identity);

        var blinkTo = Cursor.CursorPosition;

        float maxDistance = CharInfo.ActiveSkill.Value1;

        var diff = blinkTo - Crew.Instance.transform.position;

        if (diff.magnitude > maxDistance)
        {
            blinkTo = Crew.Instance.transform.position + diff.normalized * maxDistance;
        }

        blinkTo = new Vector3(Mathf.Clamp(blinkTo.x, 0.25f, float.MaxValue), Mathf.Clamp(blinkTo.y, 0.25f, float.MaxValue), 0);

        Crew.Instance.transform.position = blinkTo;

        Instantiate(BlinkOut, blinkTo, Quaternion.identity);
    }
}
