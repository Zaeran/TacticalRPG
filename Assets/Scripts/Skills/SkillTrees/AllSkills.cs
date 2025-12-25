using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSkills
{
    static List<Skill> _allSkills;

    public static void InitializeSkills()
    {
        _allSkills = new List<Skill>();

        UniversalSkills();
        BladeSkills();
        BluntSkills();
        BowSkills();
        GeomancySkills();
    }

    public static Skill GetSkillForName(string skillName)
    {
        if(_allSkills == null)
        {
            InitializeSkills();
        }
        for (int i = 0; i < _allSkills.Count; i++)
        {
            if (_allSkills[i].Name == skillName)
            {
                return _allSkills[i];
            }
        }
        return null;
    }

    #region Universal
    static void UniversalSkills()
    {
        MoveSkill();
    }

    static void MoveSkill()
    {
        Skill skill = new Skill("Move");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(1);
        skill.AddPrerequisite(prereq);
        skill.SetTags(Skill.SkillTag.Movement);
        skill.SetTargeting(new SkillTargetMoveRange());
        skill.AddSkillCost(new SkillCostSquareDistance());
        skill.AddSkillEffect(new SkillEffectMove());
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0));
        _allSkills.Add(skill);
    }
    #endregion
    #region Blade
    static void BladeSkills()
    {
        SliceSkill();
    }

    static void SliceSkill()
    {
        Skill skill = new Skill("Slice");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(2);
        skill.AddPrerequisite(prereq);
        SkillPrereqActiveSkillTree prereq2 = new SkillPrereqActiveSkillTree();
        prereq2.SetPrereqProperty(SkillTree.Blade);
        skill.AddPrerequisite(prereq2);
        skill.SetTags(Skill.SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(2);
        skill.AddSkillCost(cost);
        SkillTargetRadiusAOE skillTarget = new SkillTargetRadiusAOE(0);
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _allSkills.Add(skill);
    }
    #endregion
    #region BluntSkills
    static void BluntSkills()
    {
        WhackSkill();
        KnockbackSkill();
    }

    static void WhackSkill()
    {
        Skill skill = new Skill("Whack");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(2);
        skill.AddPrerequisite(prereq);
        SkillPrereqActiveSkillTree prereq2 = new SkillPrereqActiveSkillTree();
        prereq2.SetPrereqProperty(SkillTree.Blunt);
        skill.AddPrerequisite(prereq2);
        skill.SetTags(Skill.SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(2);
        skill.AddSkillCost(cost);
        SkillTargetRadiusAOE skillTarget = new SkillTargetRadiusAOE(0);
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _allSkills.Add(skill);
    }

    static void KnockbackSkill()
    {
        Skill skill = new Skill("Knockback");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(4);
        skill.AddPrerequisite(prereq);
        SkillPrereqActiveSkillTree prereq2 = new SkillPrereqActiveSkillTree();
        prereq2.SetPrereqProperty(SkillTree.Blunt);
        skill.AddPrerequisite(prereq2);
        skill.SetTags(Skill.SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        skill.AddSkillEffect(new SkillEffectMoveTargetBack(1));
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(4);
        skill.AddSkillCost(cost);
        SkillTargetRadiusAOE skillTarget = new SkillTargetRadiusAOE(0);
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _allSkills.Add(skill);
    }
    #endregion
    #region Bow
    public static void BowSkills()
    {
        ShootSkill();
    }
    static void ShootSkill()
    {
        Skill skill = new Skill("Shoot");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(2);
        skill.AddPrerequisite(prereq);
        SkillPrereqActiveSkillTree prereq2 = new SkillPrereqActiveSkillTree();
        prereq2.SetPrereqProperty(SkillTree.Bow);
        skill.AddPrerequisite(prereq2);
        skill.SetTags(Skill.SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(2);
        skill.AddSkillCost(cost);
        SkillTargetRadiusAOE skillTarget = new SkillTargetRadiusAOE(0);
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _allSkills.Add(skill);
    }
    #endregion
    #region Geomancy
    public static void GeomancySkills()
    {
        RaiseTerrainSkill();
        LowerTerrainSkill();
        ThrowRockSkill();
    }

    static void RaiseTerrainSkill()
    {
        Skill skill = new Skill("Raise Terrain");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(3);
        skill.AddPrerequisite(prereq);
        SkillPrereqActiveSkillTree prereq2 = new SkillPrereqActiveSkillTree();
        prereq2.SetPrereqProperty(SkillTree.Geomancy);
        skill.AddPrerequisite(prereq2);
        skill.SetTags(Skill.SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMagic(3));
        skill.AddSkillEffect(new SkillEffectRaiseTerrain());
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(2);
        skill.AddSkillCost(cost);
        SkillTargetRadiusAOE skillTarget = new SkillTargetRadiusAOE(0);
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _allSkills.Add(skill);
    }

    static void LowerTerrainSkill()
    {
        Skill skill = new Skill("Lower Terrain");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(3);
        skill.AddPrerequisite(prereq);
        SkillPrereqActiveSkillTree prereq2 = new SkillPrereqActiveSkillTree();
        prereq2.SetPrereqProperty(SkillTree.Geomancy);
        skill.AddPrerequisite(prereq2);
        skill.SetTags(Skill.SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMagic(3));
        skill.AddSkillEffect(new SkillEffectLowerTerrain());
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(2);
        skill.AddSkillCost(cost);
        SkillTargetRadiusAOE skillTarget = new SkillTargetRadiusAOE(0);
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _allSkills.Add(skill);
    }

    static void ThrowRockSkill()
    {
        Skill skill = new Skill("Throw Rock");
        SkillPrereqAPCost prereq = new SkillPrereqAPCost();
        prereq.SetPrereqProperty(3);
        skill.AddPrerequisite(prereq);
        SkillPrereqActiveSkillTree prereq2 = new SkillPrereqActiveSkillTree();
        prereq2.SetPrereqProperty(SkillTree.Geomancy);
        skill.AddPrerequisite(prereq2);
        skill.SetTags(Skill.SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMagic(3));
        skill.AddSkillEffect(new SkillEffectEarthDamage(2));
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(3);
        skill.AddSkillCost(cost);
        SkillTargetRadiusAOE skillTarget = new SkillTargetRadiusAOE(0);
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _allSkills.Add(skill);
    }
    #endregion

}
