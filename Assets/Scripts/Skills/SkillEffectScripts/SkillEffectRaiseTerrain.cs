using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectRaiseTerrain : ISkillEffect
{

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        foreach (ClickableTarget hitObject in hitCharacters)
        {
            //if(hitObject is TerrainObject)
            {
                hitObject.transform.position += new Vector3(0, 0.5f, 0);
            }
        }
    }

        public string Description(CharacterObject c)
    {
        return string.Format("Raise terrain height by 1");
    }
}