using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrereqActiveSkillTree : ISkillPrerequisite
{
    SkillTree requiredSkillTree;

    public bool SetPrereqProperty(object o)
    {
        if(o is SkillTree)
        {
            requiredSkillTree = (SkillTree)o;
            return true;
        }
        return false;
    }

    public bool MeetsPrerequisite(CharacterObject c)
    {
        return c.MyCharacter.ActiveSkillTrees.Contains(requiredSkillTree);
    }

    public string GetPrerequisiteFailureText()
    {
        return "Requires active skill tree: " + requiredSkillTree.ToString();
    }
}
