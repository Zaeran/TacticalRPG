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
            validPoints = FindValidPoints.GetPoints("Melee", c.gameObject.transform.position, c.MyCharacter.Weapon.Range, c.MyCharacter.JumpStat);
        }
        else //must be ranged
        {
            validPoints = FindValidPoints.GetPoints("Ranged", c.gameObject.transform.position, c.MyCharacter.Weapon.Range, 5);
        }
        
        DrawSquaresScript.DrawValidSquares(validPoints);
        MouseControlScript.SelectPosition(validPoints);
    }

     public string Description(CharacterObject c)
    {
        return string.Format("Weapon range ({0})", c.MyCharacter.Weapon.Range);
    }
}
