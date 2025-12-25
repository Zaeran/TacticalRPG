using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectMoveTargetBack : ISkillEffect
{

    int _knockbackDistance;

    public SkillEffectMoveTargetBack(int distance)
    {
        _knockbackDistance = distance;
    }

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        foreach (ClickableTarget hitObject in hitCharacters)
        {
            if(hitObject is CharacterObject)
            {
                if(hitObject == c) //can't knock yourself backwards
                {
                    continue;
                }
                //Get position away
                Vector2 knockBackDir = new Vector2(hitObject.transform.position.x, hitObject.transform.position.z) - new Vector2(c.transform.position.x, c.transform.position.z);
                knockBackDir = MathUtilities.NearestStraightLineVector(knockBackDir).normalized;
                hitObject.transform.position += new Vector3(knockBackDir.x, 0, knockBackDir.y) * _knockbackDistance;
            }
        }
    }
}