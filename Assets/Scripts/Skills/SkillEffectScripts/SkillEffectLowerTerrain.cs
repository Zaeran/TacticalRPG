using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectLowerTerrain : ISkillEffect
{

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        foreach (ClickableTarget hitObject in hitCharacters)
        {
            //if(hitObject is TerrainObject)
            {
                hitObject.transform.position += new Vector3(0, -1, 0);
            }
        }
    }

        public string Description(CharacterObject c)
    {
        return string.Format("Lower terrain height by 1");
    }
}