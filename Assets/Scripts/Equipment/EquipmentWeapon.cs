using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWeapon : Equipment
{
    protected int _damage;
    protected int _range;
    protected WeaponTargetingType _targetingType;

    public EquipmentWeapon(string name, List<SkillTree> allowedSkillTrees, int damage, int range, WeaponTargetingType targetType) : base(name, EquipmentType.Weapon, allowedSkillTrees)
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

    public string WeaponInformation
    {
        get
        {
            string wName = _name;
            string damage = _damage.ToString();
            string range = _range.ToString();
            string skillTrees = "";
            foreach(SkillTree s in _allowedSkillTrees)
            {
                skillTrees += "- " + s.ToString() + "\n";
            }
            return string.Format("{0}\nDAMAGE: {1}\nRANGE: {2}\nSKILL TREES\n{3}", wName, damage, range, skillTrees);
        }
    }
}
