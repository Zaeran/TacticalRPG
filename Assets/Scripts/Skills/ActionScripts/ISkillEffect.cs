using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillEffect
{
    bool PayCost(CharacterObject c, int apCost);
    bool ProcessEffect(CharacterObject c, Vector4 point);
}
