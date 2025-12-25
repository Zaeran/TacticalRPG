using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    string _characterName;
    bool _isNPC;
    int _facing = 0;
    Attributes _attributes;
    List<Skill> _skills;
    EquipmentWeapon _weapon;
    EquipmentArmour _armour;

    public event VoidEvent OnWeaponChanged;

    public Character(string cName)
    {
        _characterName = cName;
        _attributes = new Attributes();
        _skills = new List<Skill>();
        if (cName == "Char 1")
        {
            //_weapon = new EquipmentWeapon("Sword", EquipmentType.Weapon, 2, 1, WeaponTargetingType.Line);
            _weapon = new EquipmentWeapon("Sword", EquipmentType.Weapon, new List<SkillTree>() { SkillTree.Blade }, 2, 1, WeaponTargetingType.Line);
        }
        else if(cName == "Char 2")
        {
            _weapon = new EquipmentWeapon("Bow", EquipmentType.Weapon, new List<SkillTree>() { SkillTree.Bow }, 1, 4, WeaponTargetingType.Range);
        }
        else
        {
            _weapon = new EquipmentWeapon("Hydromancy Focus", EquipmentType.Weapon, new List<SkillTree>() { SkillTree.Hydromancy }, 0, 0, WeaponTargetingType.Range);
        }
       // _armour = new EquipmentArmour("Light", EquipmentType.Armour, new List<SkillTree>() { SkillTree.Armour }, 1, 1);

        SetupSkills();
    }

    void SetupSkills()
    {
        _skills.AddRange(AllSkills.GetAllSkills);
    }

    public string CharacterName
    {
        get { return _characterName; }
    }

    public bool IsNPC
    {
        get { return _isNPC; }
    }

    public bool IsPlayable
    {
        get { return !_isNPC; }
    }

    public int Facing
    {
        get { return _facing; }
    }

    public void SetFacing(int val)
    {
        _facing = val;
    }

    public Attributes Attributes
    {
        get { return _attributes; }
    }

    public int JumpStat
    {
        get { return Mathf.Clamp(Attributes.MaxJump - Armour.JumpReduction, 1, int.MaxValue); }
    }

    public List<Skill> Skills
    {
        get
        {
            return _skills;
        }
    }

    public Skill GetSkillByName(string name)
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].Name == name)
            {
                return _skills[i];
            }
        }
        return null;
    }

    public EquipmentWeapon Weapon
    {
        get
        {
            if (_weapon == null)
            {
                return new EquipmentWeapon("Unarmed", EquipmentType.Weapon, new List<SkillTree>() { SkillTree.Unarmed }, 1, 1, WeaponTargetingType.Line);
            }
            return _weapon;
        }
    }

    public void EquipNewWeapon(EquipmentWeapon weapon)
    {
        _weapon = weapon;
        if(OnWeaponChanged != null)
        {
            OnWeaponChanged();
        }
    }

    public EquipmentArmour Armour
    {
        get
        {
            if(_armour == null)
            {
                return new EquipmentArmour("Unarmoured", EquipmentType.Armour, new List<SkillTree>(), 0, 0);
            }
            return _armour;
        }
    }

    public List<SkillTree> ActiveSkillTrees
    {
        get
        {
            List<SkillTree> trees = new List<SkillTree>();
            foreach(SkillTree t in _weapon.AllowedSkillTrees)
            {
                trees.Add(t);
            }
            foreach(SkillTree t in Armour.AllowedSkillTrees)
            {
                trees.Add(t);
            }
            return trees;
        }
    }

    #region Things that happen to a character
    public int ApplyDamage(int val)
    {
        int adjustedDamage = Mathf.Clamp(val - Armour.DamageReduction, 0, int.MaxValue);
        
        AdjustHitPoints(-adjustedDamage);
        return adjustedDamage;
    }

    public void ApplyHealing(int val)
    {
        AdjustHitPoints(val);
    }

    public void AdjustHitPoints(int val)
    {
        _attributes.HealthCurrent += val;
    }
    #endregion
}
