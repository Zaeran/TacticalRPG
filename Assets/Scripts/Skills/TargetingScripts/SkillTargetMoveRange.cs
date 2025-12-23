using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetMoveRange : ISkillTargeting
{
    public void SelectTarget(CharacterObject c)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Move", c.gameObject, c.MyCharacter.Attributes.CurrentAP, c.MyCharacter.Attributes.MaxJump);
        DrawSquaresScript.DrawValidSquares(validPoints);
        MouseControlScript.SelectPosition(validPoints);
    }
}
