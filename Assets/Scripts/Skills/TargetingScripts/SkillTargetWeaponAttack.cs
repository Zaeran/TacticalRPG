using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetWeaponAttack : ISkillTargeting
{
    public void SelectTarget(CharacterObject c)
    {
        List<Vector4> validPoints = new List<Vector4>();
        if (c.MyCharacter.Weapon.TargetingType == WeaponTargetingType.Line)
        {
            validPoints = FindValidPoints.GetPoints("Melee", c.gameObject, c.MyCharacter.Weapon.Range, c.MyCharacter.Attributes.MaxJump);
        }
        else //must be ranged
        {
            validPoints = FindValidPoints.GetPoints("Ranged", c.gameObject, c.MyCharacter.Weapon.Range, c.MyCharacter.Attributes.MaxJump);
        }
        
        DrawSquaresScript.DrawValidSquares(validPoints);
        MouseControlScript.SelectPosition(validPoints);
    }
}
