using UnityEngine;
using System.Collections;

public class Punker_Sweep :  AttackSkill, AnimEventReceiver 
{
    //Set through Unity
    public Transform FistEffectTrans;

    public GameObject FistTrailEffectPrefab;
    public GameObject FistChargeEffectPrefab;

    public AnimEventRelay AnimEventRelay;
    //

    GameObject fistTrailEffect;
    GameObject fistChargeEfffect;

    string[] events;

    void Awake()
    {
        this.AnimatorAttackVar = "Sweep";

        events = new string[]
        {
            AnimatorAttackVar + "_Charge",
            AnimatorAttackVar + "_Attack",
            AnimatorAttackVar + "_Hit"
        };

    }

	// Use this for initialization
	void Start ()
    {
        foreach (var e in events)
        {
            AnimEventRelay.SubscribeToEvent(e, this);
        }

        fistTrailEffect = InstantiateEffect(FistTrailEffectPrefab, FistEffectTrans);
        fistChargeEfffect = InstantiateEffect(FistChargeEffectPrefab, FistEffectTrans);

        this.LoadSkillData(fistTrailEffect);

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
        }
    }
}
