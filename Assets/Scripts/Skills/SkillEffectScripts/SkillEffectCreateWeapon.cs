using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectCreateWeapon : ISkillEffect
{
    EquipmentWeapon _weapon;

    public SkillEffectCreateWeapon(EquipmentWeapon weapon)
    {
        _weapon = weapon;
    }

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        c.MyCharacter.EquipNewWeapon(_weapon);
    }

    public string Description(CharacterObject c)
    {
        return string.Format("Summon and equip weapon: {0}\n{1}", _weapon.Name, _weapon.WeaponInformation);
    }
}
