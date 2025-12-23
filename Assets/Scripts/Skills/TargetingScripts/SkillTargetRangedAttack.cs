using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetRangedAttack : ISkillTargeting
{
    public void SelectTarget(CharacterObject c)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Ranged", c.gameObject, 4, c.MyCharacter.Attributes.MaxJump);
        DrawSquaresScript.DrawValidSquares(validPoints);
        MouseControlScript.SelectPosition(validPoints);
    }
}
