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
        EquipmentArmour _armour;

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
        _armour = new EquipmentArmour("Light", EquipmentType.Armour, 1, 1);
        
        SetupSkills();
    }

    void SetupSkills()
    {
        Skill moveSkill = new Skill("Move", Skill.SkillTag.Movement, new SkillTargetMoveRange(), new List<ISkillEffect> () { new SkillEffectMove () });
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
                prereq.SetPrereqProperty(1);
                moveSkill.AddPrerequisite(prereq);
                _skills.Add(moveSkill);

        Skill attackSkill = new Skill("Attack", Skill.SkillTag.Attack, new SkillTargetWeaponAttack(), new List<ISkillEffect> () { new SkillEffectWeaponAttack () });
                SkillPrereqAPCost prereq2 = new SkillPrereqAPCost ();
                prereq2.SetPrereqProperty (2);
                attackSkill.AddPrerequisite (prereq2);
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

        public int JumpStat {
        get { return Mathf.Clamp(Attributes.MaxJump - _armour.JumpReduction, 0, int.MaxValue);}
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

        #region Things that happen to a character
        public void ApplyDamage (int val)
        {
                int adjustedDamage = Mathf.Clamp(val - _armour.DamageReduction, 0, int.MaxValue);
                AdjustHitPoints (-adjustedDamage);
        }

        public void ApplyHealing (int val)
        {
                AdjustHitPoints (val);
        }

        public void AdjustHitPoints (int val)
        {
                _attributes.HealthCurrent += val;
                Debug.Log ("HP After: " + _attributes.HealthCurrent);
        }
        #endregion
}
