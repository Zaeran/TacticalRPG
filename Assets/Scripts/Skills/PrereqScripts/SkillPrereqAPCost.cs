using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrereqAPCost : ISkillPrerequisite
{
    int _apCost;

    public SkillPrereqAPCost(int cost)
    {
        _apCost = cost;
    }

    public bool MeetsPrerequisite(CharacterObject c)
    {
        return c.MyCharacter.Attributes.CurrentAP >= _apCost;
    }

    public string GetPrerequisiteFailureText()
    {
        return "Not enough AP";
    }

    public string Description(CharacterObject c)
    {
        return "AP: " + _apCost.ToString();
    }
}
