using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllWeapons
{
    static List<EquipmentWeapon> _allWeapons;

    public static void Initialize()
    {
        _allWeapons = new List<EquipmentWeapon>();
        _allWeapons.Add(new EquipmentWeapon("Sword", new List<SkillTree>() { SkillTree.Blade }, 2, 1, WeaponTargetingType.Line));
        _allWeapons.Add(new EquipmentWeapon("Club", new List<SkillTree>() { SkillTree.Blunt }, 2, 1, WeaponTargetingType.Line));
        _allWeapons.Add(new EquipmentWeapon("Bow", new List<SkillTree>() { SkillTree.Bow }, 1, 5, WeaponTargetingType.Range));
        _allWeapons.Add(new EquipmentWeapon("Potion Kit", new List<SkillTree>() { SkillTree.Potions }, 0, 1, WeaponTargetingType.Line));
        _allWeapons.Add(new EquipmentWeapon("Air Focus", new List<SkillTree>() { SkillTree.Aeromancy }, 0, 0, WeaponTargetingType.Line));
        _allWeapons.Add(new EquipmentWeapon("Earth Focus", new List<SkillTree>() { SkillTree.Geomancy }, 0, 0, WeaponTargetingType.Line));
        _allWeapons.Add(new EquipmentWeapon("Energy Focus", new List<SkillTree>() { SkillTree.Pyromancy }, 0, 0, WeaponTargetingType.Line));
        _allWeapons.Add(new EquipmentWeapon("Water Focus", new List<SkillTree>() { SkillTree.Hydromancy }, 0, 0, WeaponTargetingType.Line));
    }

    public static List<EquipmentWeapon> GetAllWeapons
    {
        get
        {
            if(_allWeapons == null)
            {
                Initialize();
            }
            return _allWeapons;
        }
    }

    public static EquipmentWeapon GetWeaponByName(string wName)
    {
        foreach(EquipmentWeapon w in _allWeapons)
        {
            if(w.Name == wName)
            {
                return w;
            }
        }
        return null;
    }
}

/*
 * TEST WEAPONS
- air focus
- water focus
- energy focus (fire+electricity)
- earth focus
- sword (blade)
- club (blunt)
- bow (bow)
- potion kit (potions)
*/