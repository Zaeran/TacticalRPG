using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCostSquareDistance : ISkillCost
{

    public SkillCostSquareDistance() { }

    public bool CanPayCost(CharacterObject c, Vector4 point)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Move", c.gameObject.transform.position, c.MyCharacter.Attributes.CurrentAP, c.MyCharacter.JumpStat, false, null);
        int apCost;
        Vector3[] movePath = Pathfinding.StartPathFinding(point, validPoints, c.MyCharacter.JumpStat, c.gameObject, out apCost);

        return c.MyCharacter.Attributes.CurrentAP >= apCost;
    }

    public bool PayCost(CharacterObject c, Vector4 point)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Move", c.gameObject.transform.position, c.MyCharacter.Attributes.CurrentAP, c.MyCharacter.JumpStat, false, null);
        int apCost;
        Vector3[] movePath = Pathfinding.StartPathFinding(point, validPoints, c.MyCharacter.JumpStat, c.gameObject, out apCost);

        if (c.MyCharacter.Attributes.CurrentAP < apCost)
        {
            return false;
        }
        c.MyCharacter.Attributes.ModifyAP(-apCost);
        return true;
    }

    public string Description(CharacterObject c)
    {
        return "One AP per square";
    }
}
