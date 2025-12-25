using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetMagic : ISkillTargeting
{
    int _magicRange;

    public SkillTargetMagic(int range)
    {
        _magicRange = range;
    }

    public void SelectTarget(CharacterObject c)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Magic", c.gameObject.transform.position, _magicRange, 10);
        DrawSquaresScript.DrawValidSquares(validPoints);
        MouseControlScript.SelectPosition(validPoints);
    }
}

