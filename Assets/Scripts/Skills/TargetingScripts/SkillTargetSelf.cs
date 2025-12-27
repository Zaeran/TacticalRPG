using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetSelf : ISkillTargeting
{
    public void SelectTarget(CharacterObject c)
    {
        DrawSquaresScript.DrawValidSquares(new List<Vector4>() { c.transform.position });
        MouseControlScript.SelectPosition(new List<Vector4>() { c.transform.position });
    }

     public string Description(CharacterObject c)
    {
        return string.Format("Self");
    }
}
