using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrereqAPCost : ISkillPrerequisite
{
    int requiredAP;

    public bool SetPrereqProperty(object o)
    {
        if(o is int)
        {
            requiredAP = (int)o;
            return true;
        }
        return false;
    }

    public bool MeetsPrerequisite(CharacterObject c)
    {
        return c.MyCharacter.Attributes.CurrentAP >= requiredAP;
    }

    public string GetPrerequisiteFailureText()
    {
        return "Not enough AP";
    }
}
