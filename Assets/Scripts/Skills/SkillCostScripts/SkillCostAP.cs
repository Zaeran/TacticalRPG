using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCostAP : ISkillCost
{
    int apCost;

   public void SetCostValue(object o)
    {
        apCost = (int)o;
    }

    public bool CanPayCost(CharacterObject c, Vector4 point)
    {
        return c.MyCharacter.Attributes.CurrentAP >= apCost;
    }

    public bool PayCost(CharacterObject c, Vector4 point)
    {
        if (c.MyCharacter.Attributes.CurrentAP < apCost)
        {
            return false;
        }
        c.MyCharacter.Attributes.ModifyAP(-apCost);
        return true;
    }
}
