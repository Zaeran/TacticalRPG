using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectKnockbackFromCharacter : ISkillEffect
{
    int _knockbackDistance;

    public SkillEffectKnockbackFromCharacter(int distance)
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
                //Get direction to move
                Vector2 knockBackDir = new Vector2(hitObject.transform.position.x, hitObject.transform.position.z) - new Vector2(c.transform.position.x, c.transform.position.z);
                knockBackDir = MathUtilities.NearestStraightLineVector(knockBackDir).normalized;

                //Is there a wall in the way?
                RaycastHit hit;
                if (Physics.Raycast(hitObject.transform.position + new Vector3(0, 0.1f, 0), new Vector3(knockBackDir.x, 0, knockBackDir.y), out hit, _knockbackDistance))
                {
                    //Hit the wall instead. Deal one damage.
                    ((CharacterObject)hitObject).MyCharacter.ApplyDamage(1);
                    FloatingNumbersScript.CreateFloatingNumber(hitObject.transform.position, "1", Color.red);
                    //Put position one square in front of wall
                    hitObject.transform.position = hit.point - (new Vector3(knockBackDir.x, 0, knockBackDir.y) * 0.5f);
                }
                else
                {
                    //No wall, go backwards
                    hitObject.transform.position += new Vector3(knockBackDir.x, 0, knockBackDir.y) * _knockbackDistance;
                }
                //Return to ground
                ((CharacterObject)hitObject).Fall();
            }
        }
    }
}