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

    public Character(string cName)
    {
        _characterName = cName;
        _attributes = new Attributes();
        _skills = new List<Skill>();
        if (cName == "Char 1")
        {
            //_weapon = new EquipmentWeapon("Sword", EquipmentType.Weapon, 2, 1, WeaponTargetingType.Line);
            _weapon = new EquipmentWeapon("Spear", EquipmentType.Weapon, new List<SkillTree>() { SkillTree.Spear }, 2, 2, WeaponTargetingType.Line);
        }
        else
        {
            _weapon = new EquipmentWeapon("Bow", EquipmentType.Weapon, new List<SkillTree>() { SkillTree.Bow }, 1, 4, WeaponTargetingType.Range);
        }
        _armour = new EquipmentArmour("Light", EquipmentType.Armour, new List<SkillTree>() { SkillTree.Armour }, 1, 1);

        SetupSkills();
    }

    void SetupSkills()
    {
        CreateMoveSkill();
        CreateAttackSkill();
        CreateRaiseTerrainSkill();
    }

    void CreateMoveSkill()
    {
        Skill skill = new Skill("Move");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(1);
        skill.AddPrerequisite(prereq);
        skill.SetTags(Skill.SkillTag.Movement);
        skill.SetTargeting(new SkillTargetMoveRange());
        skill.AddSkillEffect(new SkillEffectMove());
        skill.AddSkillCost(new SkillCostSquareDistance());
        skill.SetSkillTargetRadius(new SkillTargetSingle());
        _skills.Add(skill);
    }

    void CreateAttackSkill()
    {
        Skill skill = new Skill("Attack");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(2);
        skill.AddPrerequisite(prereq);
        skill.SetTags(Skill.SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(2);
        skill.AddSkillCost(cost);
        SkillTargetAOE skillTarget = new SkillTargetAOE();
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _skills.Add(skill);
    }

    void CreateRaiseTerrainSkill()
    {
        Skill skill = new Skill("Raise Terrain");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(1);
        skill.AddPrerequisite(prereq);
        skill.SetTags(Skill.SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMoveRange());
        skill.AddSkillEffect(new SkillEffectRaiseTerrain());
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(1);
        skill.AddSkillCost(cost);
        SkillTargetAOE skillTarget = new SkillTargetAOE();
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _skills.Add(skill);
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
        get { return Mathf.Clamp(Attributes.MaxJump - _armour.JumpReduction, 0, int.MaxValue); }
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

    #region Things that happen to a character
    public int ApplyDamage(int val)
    {
        int adjustedDamage = Mathf.Clamp(val - _armour.DamageReduction, 0, int.MaxValue);
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
        Debug.Log("HP After: " + _attributes.HealthCurrent);
    }
    #endregion
}
