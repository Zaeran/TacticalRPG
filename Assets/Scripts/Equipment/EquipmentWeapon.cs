using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWeapon : Equipment
{
    protected int _damage;
    protected int _range;
    protected WeaponTargetingType _targetingType;

    public EquipmentWeapon(string name, EquipmentType eType, int damage, int range, WeaponTargetingType targetType) : base(name, eType)
    {
        _damage = damage;
        _range = range;
        _targetingType = targetType;
    }
}
