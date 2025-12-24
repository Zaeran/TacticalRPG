using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentArmour : Equipment
{
    protected int _damageReduction;
    protected int _jumpReduction;

    public EquipmentArmour(string name, EquipmentType eType, int damageReduction, int jumpReduction) : base(name, eType)
    {
        _damageReduction = damageReduction;
        _jumpReduction = jumpReduction;
    }

    public int DamageReduction
    {
        get { return _damageReduction; }
    }

    public int JumpReduction
    {
        get { return _jumpReduction; }
    }
}
