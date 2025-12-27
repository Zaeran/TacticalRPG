using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillTargeting
{
    void SelectTarget(CharacterObject c);
    string Description(CharacterObject c);
}
