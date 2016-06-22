using UnityEngine;
using System.Collections;

[System.Serializable]
public class Damage
{
    public int Amount;
    public DamageType Type;

    public Damage()
    {
    }

    public Damage(int amount, DamageType type)
    {
        Amount = amount;
        Type = type;
    }
}

public enum DamageType
{
    Kinetic,
    Fire,
    Electrical
}
