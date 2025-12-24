using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillPrerequisite
{
    bool SetPrereqProperty(object o);
    bool MeetsPrerequisite(CharacterObject c);
}
