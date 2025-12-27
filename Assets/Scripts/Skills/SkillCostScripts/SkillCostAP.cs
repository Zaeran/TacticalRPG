using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCostAP : ISkillCost
{
    int _apCost;

   public SkillCostAP(int cost)
    {
        _apCost = cost;
    }

    public bool CanPayCost(CharacterObject c, Vector4 point)
    {
        return c.MyCharacter.Attributes.CurrentAP >= _apCost;
    }

    public bool PayCost(CharacterObject c, Vector4 point)
    {
        if (c.MyCharacter.Attributes.CurrentAP < _apCost)
        {
            return false;
        }
        c.MyCharacter.Attributes.ModifyAP(-_apCost);
        return true;
    }

    public string Description(CharacterObject c)
    {
        return "AP: " + _apCost.ToString();
    }
}
