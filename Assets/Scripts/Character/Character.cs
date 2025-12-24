using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    string _characterName;
    bool _isNPC;
    Attributes _attributes;
    List<Skill> _skills;
    EquipmentWeapon _weapon;

    public Character(string cName)
    {
        _characterName = cName;
        _attributes = new Attributes();
        _skills = new List<Skill>();
        if(cName == "Char 1")
        {
            //_weapon = new EquipmentWeapon("Sword", EquipmentType.Weapon, 2, 1, WeaponTargetingType.Line);
            _weapon = new EquipmentWeapon("Spear", EquipmentType.Weapon, 2, 2, WeaponTargetingType.Line);
        }
        else
        {
            _weapon = new EquipmentWeapon("Bow", EquipmentType.Weapon, 1, 4, WeaponTargetingType.Range);
        }
        
        SetupSkills();
    }

    void SetupSkills()
    {
        Skill moveSkill = new Skill("Move", Skill.SkillTag.Movement, new SkillTargetMoveRange(), new List<ISkillEffect> () { new SkillEffectMove () });
        _skills.Add(moveSkill);

        Skill attackSkill = new Skill("Attack", Skill.SkillTag.Attack, new SkillTargetWeaponAttack(), new List<ISkillEffect> () { new SkillEffectWeaponAttack () });
        _skills.Add(attackSkill);
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

    public Attributes Attributes
    {
        get { return _attributes; }
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
        for(int i = 0; i < _skills.Count; i++)
        {
            if(_skills[i].Name == name)
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
            if(_weapon == null)
            {
                return new EquipmentWeapon("Unarmed", EquipmentType.Weapon, 1, 1, WeaponTargetingType.Line);
            }
            return _weapon;
        }
    }
}
