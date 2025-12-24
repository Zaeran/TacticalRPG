using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectWeaponAttack : ISkillEffect
{

    public void ProcessEffect(CharacterObject c, List<CharacterObject> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
                foreach(CharacterObject hitCharacter in hitCharacters) {
                        int totalDamage = hitCharacter.MyCharacter.ApplyDamage (c.MyCharacter.Weapon.Damage);
            FloatingNumbersScript.CreateFloatingNumber(hitCharacter.transform.position, totalDamage.ToString(), Color.red);
        }
    }
}
