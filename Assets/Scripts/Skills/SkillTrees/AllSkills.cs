using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSkills
{
    static List<Skill> _allSkills;

    public static void InitializeSkills()
    {
        _allSkills = new List<Skill>();

        SliceSkill();
        ShootSkill();
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
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        SkillCostAP cost = new SkillCostAP();
        cost.SetCostValue(2);
        skill.AddSkillCost(cost);
        SkillTargetRadiusAOE skillTarget = new SkillTargetRadiusAOE(0);
        skillTarget.SetAOE(0);
        skill.SetSkillTargetRadius(skillTarget);
        _allSkills.Add(skill);
    }

}
