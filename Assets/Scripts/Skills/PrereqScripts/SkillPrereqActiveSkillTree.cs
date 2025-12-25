using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrereqActiveSkillTree : ISkillPrerequisite
{
    SkillTree _requiredSkillTree;

    public SkillPrereqActiveSkillTree(SkillTree skillTree)
    {
        _requiredSkillTree = skillTree;
    }

    public bool MeetsPrerequisite(CharacterObject c)
    {
        return c.MyCharacter.ActiveSkillTrees.Contains(_requiredSkillTree);
    }

    public string GetPrerequisiteFailureText()
    {
        return "Requires active skill tree: " + _requiredSkillTree.ToString();
    }
}
