using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetMagicCharacter : ISkillTargeting
{
    int _magicRange;
    bool _canTargetSelf;

    public SkillTargetMagicCharacter(int range, bool canTargetSelf)
    {
        _magicRange = range;
        _canTargetSelf = canTargetSelf;
    }

    public void SelectTarget(CharacterObject c)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Magic", c.gameObject.transform.position, _magicRange, 10, _canTargetSelf, null);
        DrawSquaresScript.DrawValidSquares(validPoints);
        MouseControlScript.SelectPosition(validPoints);
    }

    public string Description(CharacterObject c)
    {
        return string.Format("{0}{1}", _magicRange, _canTargetSelf ? "(S)" : "");
    }
}

