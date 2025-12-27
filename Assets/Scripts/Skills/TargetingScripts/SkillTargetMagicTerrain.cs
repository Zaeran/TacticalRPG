using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetMagicTerrain : ISkillTargeting
{
    int _magicRange;
    List<TerrainType> _allowedTerrain;

    public SkillTargetMagicTerrain(int range, List<TerrainType> allowedTerrain)
    {
        _magicRange = range;
        _allowedTerrain = allowedTerrain;
    }

    public void SelectTarget(CharacterObject c)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Magic", c.gameObject.transform.position, _magicRange, 10, false, _allowedTerrain);
        DrawSquaresScript.DrawValidSquares(validPoints);
        MouseControlScript.SelectPosition(validPoints);
    }

    public string Description(CharacterObject c)
    {
        return string.Format("{0} - Terrain only", _magicRange);
    }
}

