using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWeapon : Equipment
{
    protected int _damage;
    protected int _range;
    protected WeaponTargetingType _targetingType;

    public EquipmentWeapon(string name, EquipmentType eType, List<SkillTree> allowedSkillTrees, int damage, int range, WeaponTargetingType targetType) : base(name, eType, allowedSkillTrees)
    {
        _damage = damage;
        _range = range;
        _targetingType = targetType;
    }

    public int Damage
    {
        get { return _damage; }
    }

    public int Range
    {
        get { return _range; }
    }

    public WeaponTargetingType TargetingType
    {
        get { return _targetingType; }
    }
}
