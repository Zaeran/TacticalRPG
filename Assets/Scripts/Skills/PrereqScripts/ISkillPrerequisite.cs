using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillPrerequisite
{
    bool MeetsPrerequisite(CharacterObject c);
    string GetPrerequisiteFailureText();
}
