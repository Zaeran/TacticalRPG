using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillCost
{
    bool CanPayCost(CharacterObject o, Vector4 point);
   bool PayCost(CharacterObject c, Vector4 point);
}
