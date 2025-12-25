using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectMoveTargetBack : ISkillEffect
{

    int knockbackDistance;

    public SkillEffectMoveTargetBack(int distance)
    {
        knockbackDistance = distance;
    }

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        foreach (ClickableTarget hitObject in hitCharacters)
        {
            if(hitObject is CharacterObject)
            {
                //Get position away
                Vector2 knockBackDir = new Vector2(point.x, point.z) - new Vector2(c.transform.position.x, c.transform.position.z);
                knockBackDir = knockBackDir.normalized;
                hitObject.transform.position += new Vector3(knockBackDir.x, 0, knockBackDir.y) * knockbackDistance;
            }
        }
    }
}