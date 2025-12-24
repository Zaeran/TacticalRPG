using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCostSquareDistance : ISkillCost
{

   public void SetCostValue(object o)
    {
        
    }

    public bool CanPayCost(CharacterObject c, Vector4 point)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Move", c.gameObject, c.MyCharacter.Attributes.CurrentAP, c.MyCharacter.JumpStat);
        Vector3[] movePath = Pathfinding.StartPathFinding(point, validPoints, c.MyCharacter.JumpStat, c.gameObject);

        int apCost = movePath.Length - 1;
        return c.MyCharacter.Attributes.CurrentAP >= apCost;
    }

    public bool PayCost(CharacterObject c, Vector4 point)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Move", c.gameObject, c.MyCharacter.Attributes.CurrentAP, c.MyCharacter.JumpStat);
        Vector3[] movePath = Pathfinding.StartPathFinding(point, validPoints, c.MyCharacter.JumpStat, c.gameObject);

        int apCost = movePath.Length - 1;

        if (c.MyCharacter.Attributes.CurrentAP < apCost)
        {
            return false;
        }
        c.MyCharacter.Attributes.ModifyAP(-apCost);
        return true;
    }
}
