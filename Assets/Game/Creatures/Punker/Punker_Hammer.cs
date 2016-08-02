using UnityEngine;
using System.Collections;

public class Punker_Hammer : AttackSkill, AnimEventReceiver
{
    //Set through Unity
    public Animator Animator;

    public Transform FistEffectTrans;

    public GameObject FistTrailEffectPrefab;
    public GameObject FistChargeEffectPrefab;
    public GameObject GroundPoundEffectPrefab;

    public AnimEventRelay AnimEventRelay;
    //

    GameObject fistTrailEffect;
    GameObject fistChargeEfffect;
    GameObject groundPoundEffect;

    const string ATTACK_STATE = "Hammer";

    void Start()
    {
        AnimEventRelay.SubscribeToEvent("Hammer_Charge", this);
        AnimEventRelay.SubscribeToEvent("Hammer_Attack", this);
        AnimEventRelay.SubscribeToEvent("Hammer_Hit", this);

        fistTrailEffect = InstantiateEffect(FistTrailEffectPrefab, FistEffectTrans);
        fistChargeEfffect = InstantiateEffect(FistChargeEffectPrefab, FistEffectTrans);

        groundPoundEffect = (GameObject) Instantiate(GroundPoundEffectPrefab);

        this.LoadSkillData(fistTrailEffect);
    }

    void OnDestroy()
    {
        AnimEventRelay.UnsubscribeToAll(this);
    }

    GameObject InstantiateEffect(GameObject prefab, Transform trans)
    {
        var effect = (GameObject) Instantiate(prefab);
        effect.transform.SetParent(trans.parent);
        effect.transform.xCloneTransformFrom(trans);
        effect.SetActive(false);
        return effect;
    }

    public void ReceiveEvent(string name)
    {
        if (name == "Hammer_Charge")
        {
            fistChargeEfffect.SetActive(true);

            fistChargeEfffect.transform.SetParent(FistEffectTrans.parent);
            fistChargeEfffect.transform.xCloneTransformFrom(FistEffectTrans);
            fistChargeEfffect.transform.xResetParticleSystemsRecursive();

            fistChargeEfffect.SetActive(true);
            fistTrailEffect.SetActive(true);
        }
        if (name == "Hammer_Attack")
        {
            fistChargeEfffect.transform.SetParent(null);
        }
        else if (name == "Hammer_Hit")
        {
            fistTrailEffect.SetActive(false);

            groundPoundEffect.transform.position = FistEffectTrans.position;

//            groundPoundEffect.transform.SetParent(FistEffectTrans.parent);
//            groundPoundEffect.transform.xCloneTransformFrom(FistEffectTrans);
            groundPoundEffect.transform.xResetParticleSystemsRecursive();

            groundPoundEffect.SetActive(groundPoundEffect);
        }
    }


    public override void Activate()
    {
        Animator.SetBool(ATTACK_STATE, true);
    }

    public override void Deactivate()
    {
        Animator.SetBool(ATTACK_STATE, false);
    }
}
