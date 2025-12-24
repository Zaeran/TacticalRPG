using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectMove : ISkillEffect
{
    public bool PayCost(CharacterObject c, int apCost)
    {
        Debug.Log("Cost: " + apCost);
        if (c.MyCharacter.Attributes.CurrentAP < apCost)
        {
            Debug.Log("Not enough AP!");
            return false;
        }
        c.MyCharacter.Attributes.ModifyAP(-apCost);
        return true;
    }

    public void ProcessEffect(CharacterObject c, List<CharacterObject> hitCharacters, Vector4 point)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Move", c.gameObject, c.MyCharacter.Attributes.CurrentAP, c.MyCharacter.JumpStat);
        Vector3[] movePath = Pathfinding.StartPathFinding(point, validPoints, c.MyCharacter.JumpStat, c.gameObject);

        int apCost = movePath.Length - 1;

        if (!PayCost(c, apCost))
        {
            //return false;
        }
        if (movePath.Length > 0)
        {
            DrawSquaresScript.DestroyValidSquares();
            c.GetComponent<MovementScript>().MoveToPoint(c, movePath);
        }
        //return true;
    }
}
