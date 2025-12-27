using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillTargetLineDistance : ISkillTargeting
{
int _range;

public SkillTargetLineDistance(int range)
    {
        _range = range;
    }

    public void SelectTarget(CharacterObject c)
    {
        List<Vector4> validPoints = new List<Vector4>();
        validPoints = FindValidPoints.GetPoints("Melee", c.gameObject.transform.position, _range, c.MyCharacter.JumpStat, false, null);
        DrawSquaresScript.DrawValidSquares(validPoints);
        MouseControlScript.SelectPosition(validPoints);
    }

     public string Description(CharacterObject c)
    {
        return string.Format("Line - {0} squares", _range);
    }
}
