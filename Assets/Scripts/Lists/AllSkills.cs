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
        AeromancySkills();
        BladeSkills();
        BluntSkills();
        BowSkills();
        GeomancySkills();
        HydromancySkills();
        PotionSkills();
        ShieldSkills();
        UnarmedSkills();
    }

    public static Skill GetSkillForName(string skillName)
    {
        
        for (int i = 0; i < _allSkills.Count; i++)
        {
            if (_allSkills[i].Name == skillName)
            {
                return _allSkills[i];
            }
        }
        return null;
    }

    public static List<Skill> GetAllSkills
    {
        get
        {
            if (_allSkills == null)
            {
                InitializeSkills();
            }
            return _allSkills;
        }
    }

    #region Universal
    static void UniversalSkills()
    {
        MoveSkill();

    }

    static void MoveSkill()
    {
        Skill skill = new Skill("Move");
        skill.AddPrerequisite(new SkillPrereqAPCost(1));
        skill.AddSkillTags(SkillTag.Movement);
        skill.SetTargeting(new SkillTargetMoveRange());
        skill.AddSkillCost(new SkillCostSquareDistance());
        skill.AddSkillEffect(new SkillEffectMove());
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, true, false, false, false));
        _allSkills.Add(skill);
    }
    #endregion
    #region Aeromancy
    static void AeromancySkills()
    {
        Airburst();
        AirPull();
        AirPush();
    }

    public static void Airburst()
    {
        Skill skill = new Skill("Airburst");
        skill.AddPrerequisite(new SkillPrereqAPCost(4));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Aeromancy));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetSelf());
        skill.AddSkillEffect(new SkillEffectKnockbackFromCharacter(2));
        skill.AddSkillCost(new SkillCostAP(4));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(2, false, true, true, false));
        _allSkills.Add(skill);
    }

    public static void AirPull()
    {
        Skill skill = new Skill("Air Pull");
        skill.AddPrerequisite(new SkillPrereqAPCost(3));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Aeromancy));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMagic(4, false));
        skill.AddSkillEffect(new SkillEffectPullTowardsCharacter(1));
        skill.AddSkillCost(new SkillCostAP(3));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }

    public static void AirPush()
    {
        Skill skill = new Skill("Air Push");
        skill.AddPrerequisite(new SkillPrereqAPCost(3));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Aeromancy));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMagic(4, false));
        skill.AddSkillEffect(new SkillEffectKnockbackFromCharacter(1));
        skill.AddSkillCost(new SkillCostAP(3));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }
    #endregion
    #region Blade
    static void BladeSkills()
    {
        SliceSkill();
        TripleSlice();
    }

    static void SliceSkill()
    {
        Skill skill = new Skill("Slice");
        skill.AddPrerequisite(new SkillPrereqAPCost(2));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Blade));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        skill.AddSkillCost(new SkillCostAP(2));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }

    static void TripleSlice()
    {
        Skill skill = new Skill("Triple lice");
        skill.AddPrerequisite(new SkillPrereqAPCost(5));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Blade));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        skill.AddSkillCost(new SkillCostAP(5));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
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
        skill.AddPrerequisite(new SkillPrereqAPCost(2));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Blunt));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        skill.AddSkillCost(new SkillCostAP(2));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }

    static void KnockbackSkill()
    {
        Skill skill = new Skill("Knockback");
        skill.AddPrerequisite(new SkillPrereqAPCost(4));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Blunt));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        skill.AddSkillEffect(new SkillEffectKnockbackFromCharacter(1));
        skill.AddSkillCost(new SkillCostAP(4));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }
    #endregion
    #region Bow
    public static void BowSkills()
    {
        ShootSkill();
        PointBlankShot();
    }
    static void ShootSkill()
    {
        Skill skill = new Skill("Shoot");
        skill.AddPrerequisite(new SkillPrereqAPCost(2));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Bow));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectWeaponAttack());
        skill.AddSkillCost(new SkillCostAP(2));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }

    static void PointBlankShot()
    {
         Skill skill = new Skill("Point Blank Shot");
        skill.AddPrerequisite(new SkillPrereqAPCost(2));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Bow));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetLineDistance(1));
        skill.AddSkillEffect(new SkillEffectWeaponAttackMultiplier(2));
        skill.AddSkillCost(new SkillCostAP(2));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
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
        skill.AddPrerequisite(new SkillPrereqAPCost(3));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Geomancy));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMagic(3, true));
        skill.AddSkillEffect(new SkillEffectRaiseTerrain());
        skill.AddSkillCost(new SkillCostAP(2));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, true, true, true, true));
        _allSkills.Add(skill);
    }

    static void LowerTerrainSkill()
    {
        Skill skill = new Skill("Lower Terrain");
        skill.AddPrerequisite(new SkillPrereqAPCost(3));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Geomancy));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMagic(3, true));
        skill.AddSkillEffect(new SkillEffectLowerTerrain());
        skill.AddSkillCost(new SkillCostAP(3));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, true, true, true, true));
        _allSkills.Add(skill);
    }

    static void ThrowRockSkill()
    {
        Skill skill = new Skill("Throw Rock");
        skill.AddPrerequisite(new SkillPrereqAPCost(3));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Geomancy));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMagic(3, false));
        skill.AddSkillEffect(new SkillEffectEarthDamage(2));
        skill.AddSkillCost(new SkillCostAP(3));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }
    #endregion

    #region Hydromancy
    static void HydromancySkills()
    {
        CreateIceBlade();
        WaterHeal();
    }

    static void CreateIceBlade()
    {
        Skill skill = new Skill("Create Ice Blade");
        skill.AddPrerequisite(new SkillPrereqAPCost(5));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Hydromancy));
        skill.AddSkillTags(SkillTag.Magical);
        skill.SetTargeting(new SkillTargetSelf());
        skill.AddSkillEffect(new SkillEffectCreateWeapon(new EquipmentWeapon("Ice blade", new List<SkillTree>() { SkillTree.Hydromancy, SkillTree.Blade }, 2, 1, WeaponTargetingType.Line)));
        skill.AddSkillCost(new SkillCostAP(5));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, false, true));
        _allSkills.Add(skill);
    }

    static void WaterHeal()
    {
          Skill skill = new Skill("Water Heal");
        skill.AddPrerequisite(new SkillPrereqAPCost(3));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Hydromancy));
        skill.AddSkillTags(SkillTag.Buff);
        skill.SetTargeting(new SkillTargetMagic(1, true));
        skill.AddSkillEffect(new SkillEffectHealing(1));
        skill.AddSkillCost(new SkillCostAP(3));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, true));
        _allSkills.Add(skill);
    }

    static void IceBall()
    {
          Skill skill = new Skill("Ice Ball");
        skill.AddPrerequisite(new SkillPrereqAPCost(3));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Hydromancy));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetMagic(3, false));
        skill.AddSkillEffect(new SkillEffectEarthDamage(2));
        skill.AddSkillCost(new SkillCostAP(3));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }
    #endregion
    #region Potions
    static void PotionSkills()
    {
        //healing potion skill
        HealthPotion();
        BombPotion();
        //poison skill
    }

    static void HealthPotion()
    {
        Skill skill = new Skill("Healing Potion");
        skill.AddPrerequisite(new SkillPrereqAPCost(3));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Potions));
        skill.AddSkillTags(SkillTag.Buff);
        skill.SetTargeting(new SkillTargetMagic(1, true));
        skill.AddSkillEffect(new SkillEffectHealing(2));
        skill.AddSkillCost(new SkillCostAP(3));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, true));
        _allSkills.Add(skill);
    }

    static void BombPotion()
    { 
        Skill skill = new Skill("Bomb");
        skill.AddPrerequisite(new SkillPrereqAPCost(5));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Potions));
        skill.AddSkillTags(SkillTag.Buff);
        skill.SetTargeting(new SkillTargetMagic(3, true));
        skill.AddSkillEffect(new SkillEffectEarthDamage(2));
        skill.AddSkillCost(new SkillCostAP(5));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(1, false, true, true, true));
        _allSkills.Add(skill);
    }
    #endregion

    #region Shield
    static void ShieldSkills()
    {
        //block skill
        //blocking is a status effect
    }
    #endregion

    #region Unarmed
    static void UnarmedSkills()
    {
           PunchSkill();
           FlurryOfBlows();
    }

    static void PunchSkill()
    {
        Skill skill = new Skill("Punch");
        skill.AddPrerequisite(new SkillPrereqAPCost(2));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Unarmed));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectUnarmedDamage());
        skill.AddSkillCost(new SkillCostAP(2));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }

    static void FlurryOfBlows()
    {
        Skill skill = new Skill("Punch");
        skill.AddPrerequisite(new SkillPrereqAPCost(5));
        skill.AddPrerequisite(new SkillPrereqActiveSkillTree(SkillTree.Unarmed));
        skill.AddSkillTags(SkillTag.Attack);
        skill.SetTargeting(new SkillTargetWeaponAttack());
        skill.AddSkillEffect(new SkillEffectUnarmedDamage());
        skill.AddSkillEffect(new SkillEffectUnarmedDamage());
        skill.AddSkillEffect(new SkillEffectUnarmedDamage());
        skill.AddSkillCost(new SkillCostAP(5));
        skill.SetSkillTargetRadius(new SkillTargetRadiusAOE(0, false, true, true, false));
        _allSkills.Add(skill);
    }
    #endregion
}
