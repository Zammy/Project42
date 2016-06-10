using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMember : MonoBehaviour
{
    public GameObject BlinkInPrefab;
    public GameObject BlinkOutPrefab;
    public GameObject ShieldPrefab;
    public GameObject FakeTargetPrefab;

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
                this.Shield();
                break;
            case ActiveSkillType.FakeTarget:
                this.FakeTarget();
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
        Instantiate(BlinkInPrefab, transform.position, Quaternion.identity);

        var blinkTo = Cursor.CursorPosition;

        float maxDistance = CharInfo.ActiveSkill.Value1;

        var diff = blinkTo - Crew.Instance.transform.position;

        if (diff.magnitude > maxDistance)
        {
            blinkTo = Crew.Instance.transform.position + diff.normalized * maxDistance;
        }

        blinkTo = new Vector3(Mathf.Clamp(blinkTo.x, 0.25f, float.MaxValue), Mathf.Clamp(blinkTo.y, 0.25f, float.MaxValue), 0);

        Crew.Instance.transform.position = blinkTo;

        Instantiate(BlinkOutPrefab, blinkTo, Quaternion.identity);
    }

    void Shield()
    {
        var shieldGo = (GameObject) Instantiate(ShieldPrefab, transform.position, Quaternion.identity);
        shieldGo.transform.SetParent(Crew.Instance.transform);
        shieldGo.transform.localScale = Vector3.one;
        shieldGo.transform.localPosition = Vector3.zero;
        shieldGo.transform.rotation = Quaternion.Euler(0, 0, -90) * Crew.Instance.transform.localRotation;

        float duration = CharInfo.ActiveSkill.Value1;

        foreach (var member in Crew.Instance.CrewMembers)
        {
            member.CharHealth.IsShielded = true;
        }

        StartCoroutine(RemoveShieldAfter(duration));
    }

    IEnumerator RemoveShieldAfter(float sec)
    {
        yield return new WaitForSeconds(sec);

        foreach (var member in Crew.Instance.CrewMembers)
        {
            member.CharHealth.IsShielded = false;
        }
    }

    void FakeTarget()
    {
        var fakeTargetGo = (GameObject) Instantiate(FakeTargetPrefab, Cursor.CursorPosition, Quaternion.identity);
        float duration = CharInfo.ActiveSkill.Value1;
        
        if (Crew.Instance.FakeTargetPos == null)
            Crew.Instance.FakeTargetPos = new Vector3();

        Crew.Instance.FakeTargetPos = Cursor.CursorPosition;

        StartCoroutine(RemoveFakeTarget(duration));
    }

    IEnumerator RemoveFakeTarget(float sec)
    {
        yield return new WaitForSeconds(sec);

        Crew.Instance.FakeTargetPos = null;
    }
}
