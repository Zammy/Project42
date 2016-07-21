using UnityEngine;
using System.Collections;

public class CharacterSkills : MonoBehaviour 
{
    public SkillData[] Skills;
    public GameObject AttackHitEffectPrefab;
    public Transform Face;
    public Transform Nuzzle;
    public Animator Animator;

    CharacterHealth charHealth;
    CharacterMovement movement;
    PlayerInput playerInput;

    public bool CastingSkill { get; set; }

    void Start()
    {
        charHealth = GetComponent<CharacterHealth>();
        playerInput = GetComponent<PlayerInput>();
        movement = GetComponent<CharacterMovement>();
    }

    public void ExecuteSkill(int index)
    {
        if (Skills.Length <= index)
            return;

        var skillData = Skills[index];

        if (this.CastingSkill)
            return;

        StartCoroutine( ExecuteSkill (skillData) );
    }

    IEnumerator ExecuteSkill(SkillData skillData)
    {
        if (charHealth.Health <= 0)
            yield break;

        this.CastingSkill = true;

        var meleeAttack = skillData as MeleeAttackSkillData; 
        if (meleeAttack != null)
        {
            yield return StartCoroutine(AttackSkill(meleeAttack));
        }

        var dodgeSkill = skillData as DodgeSkillData;
        if (dodgeSkill != null
             && movement.MovementDirection != Vector3.zero)
        {
            yield return StartCoroutine(DodgeSkill(dodgeSkill));
        }

        var rangeAttack = skillData as RangeAttackData;
        if (rangeAttack != null)
        {
            yield return StartCoroutine(RangeAttack(rangeAttack));
        }

        this.CastingSkill = false;
    }

    IEnumerator WaitForCastTime(SkillData skillData)
    {
        if (skillData.CastTime_AnimTrigger != string.Empty)
        {
            Animator.SetTrigger(skillData.CastTime_AnimTrigger);
        }
        yield return new WaitForSeconds(skillData.CastTime);
    }

    IEnumerator WaitForWindTime(SkillData skillData)
    {
        if (skillData.WindTime_AnimTrigger != string.Empty)
        {
            Animator.SetTrigger(skillData.WindTime_AnimTrigger);
        }
        yield return new WaitForSeconds(skillData.WindTime);
    }

    IEnumerator AttackSkill(MeleeAttackSkillData attackSkill)
    {
        movement.MovementDirection = Vector3.zero;

        Animator.SetTrigger(attackSkill.Name);

        if (playerInput)
            playerInput.enabled = false;

        yield return StartCoroutine(WaitForCastTime(attackSkill));

        if (charHealth.Health <= 0)
            yield break;

        var instantiatePos = Face.transform.position + Face.transform.forward * attackSkill.Depth / 2;
        var attackHitEffectGo = (GameObject)Instantiate(AttackHitEffectPrefab, instantiatePos, Quaternion.identity);
        attackHitEffectGo.transform.localScale = new Vector3(attackSkill.Breadth, 1, attackSkill.Depth);

        var hitEffect = attackHitEffectGo.GetComponent<HitEffect>();
        hitEffect.Life = attackSkill.AttackTime;

        SetDamage(attackHitEffectGo, attackSkill.Damage);

        SetForce(attackHitEffectGo, attackSkill.Force);

        attackHitEffectGo.transform.localRotation = Quaternion.LookRotation(this.transform.forward);

        yield return StartCoroutine(WaitForWindTime(attackSkill));

        if (playerInput)
            playerInput.enabled = true;
    }

    IEnumerator DodgeSkill(DodgeSkillData dodgeSkill)
    {
        if (playerInput)
            playerInput.enabled = false;

        movement.SetMovementMode(dodgeSkill.MoveMode);
        movement.MovementDirection = this.transform.forward;

        yield return StartCoroutine(WaitForCastTime(dodgeSkill));
        if (charHealth.Health <= 0)
            yield break;

        yield return StartCoroutine(WaitForWindTime(dodgeSkill));

        if (playerInput)
            playerInput.enabled = true;

        movement.ResetMovementMode();
    }

    IEnumerator RangeAttack(RangeAttackData rangeAttack)
    {
        yield return StartCoroutine(WaitForCastTime(rangeAttack));
        if (charHealth.Health <= 0)
            yield break;

        var projectileGo = (GameObject)Instantiate(rangeAttack.ProjectilePrefab, this.Nuzzle.position, this.Nuzzle.transform.rotation);

        SetDamage(projectileGo, rangeAttack.Damage);
        SetForce(projectileGo, rangeAttack.Force);

        var projectile = projectileGo.GetComponent<Projectile>();
        projectile.SourceTag = gameObject.tag;

        yield return StartCoroutine(WaitForWindTime(rangeAttack));
    }

    void SetDamage(GameObject attackGo, Damage[] damages)
    {
        var dmgDealer = attackGo.GetComponent<DamageDealer>();
        for (int i = 0; i < damages.Length; i++)
        {
            dmgDealer.AddDamage(damages[i]);
        }
        dmgDealer.SourceTag = this.gameObject.tag;
    }

    void SetForce(GameObject attackGo, float force)
    {
        var forceDealer = attackGo.GetComponent<ForceDealer>();
        forceDealer.Force = force;
        forceDealer.SourceTag = this.gameObject.tag;
    }
}
