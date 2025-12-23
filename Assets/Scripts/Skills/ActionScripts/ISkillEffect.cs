using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillEffect
{
    bool PayCost(CharacterObject c, int apCost);
    void ProcessEffect(CharacterObject c, Vector4 point);
}
