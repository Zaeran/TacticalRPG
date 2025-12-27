using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectWeaponAttack : ISkillEffect
{

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        foreach (ClickableTarget clickable in hitCharacters)
        {
            if (clickable is CharacterObject)
            {
                CharacterObject hitCharacter = clickable as CharacterObject;
                int totalDamage = hitCharacter.MyCharacter.ApplyDamage(c.MyCharacter.Weapon.Damage);
                FloatingNumbersScript.CreateFloatingNumber(hitCharacter.transform.position, totalDamage.ToString(), Color.red);
            }
        }
    }

        public string Description(CharacterObject c)
    {
        return string.Format("Deal damage equal to your weapon's attack stat ({0})", c.MyCharacter.Weapon.Damage);
    }
}
