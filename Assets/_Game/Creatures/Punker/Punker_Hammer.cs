using UnityEngine;
using System.Collections;

public class Punker_Hammer : AttackSkill, AnimEventReceiver
{
    //Set through Unity
    public Transform FistEffectTrans;

    public GameObject FistTrailEffectPrefab;
    public GameObject FistChargeEffectPrefab;
    public GameObject GroundPoundEffectPrefab;

    public AnimEventRelay AnimEventRelay;
    //

    string[] events;


    GameObject fistTrailEffect;
    GameObject fistChargeEfffect;
    DamageEffect groundPoundEffect;

    void Awake()
    {
        this.AnimatorAttackVar = "Hammer";

        events = new string[]
        {
            AnimatorAttackVar + "_Charge",
            AnimatorAttackVar + "_Attack",
            AnimatorAttackVar + "_Hit"
        };
    }

    protected override void Start()
    {
        base.Start();

        foreach (var e in events)
        {
            AnimEventRelay.SubscribeToEvent(e, this);
        }

        fistTrailEffect = InstantiateEffect(FistTrailEffectPrefab, FistEffectTrans);
        fistChargeEfffect = InstantiateEffect(FistChargeEffectPrefab, FistEffectTrans);

        var groundPoundEffectGo = (GameObject) Instantiate(GroundPoundEffectPrefab);
        groundPoundEffectGo.SetActive(false);

        this.LoadSkillData(groundPoundEffectGo);

        groundPoundEffect = groundPoundEffectGo.GetComponent<DamageEffect>();
    }

    void OnDestroy()
    {
        AnimEventRelay.UnsubscribeToAll(this);
    }

    public void ReceiveEvent(string name)
    {
        if (name == events[0])
        {
            fistChargeEfffect.SetActive(true);

            fistChargeEfffect.transform.SetParent(FistEffectTrans.parent);
            fistChargeEfffect.transform.xCloneTransformFrom(FistEffectTrans);
            fistChargeEfffect.transform.xResetParticleSystemsRecursive();

            fistChargeEfffect.SetActive(true);
            fistTrailEffect.SetActive(true);
        }
        if (name == events[1])
        {
            fistChargeEfffect.transform.SetParent(null);
        }
        else if (name == events[2])
        {
            fistTrailEffect.SetActive(false);

            groundPoundEffect.transform.position = FistEffectTrans.position;

            groundPoundEffect.Activate();
        }
    }
}
