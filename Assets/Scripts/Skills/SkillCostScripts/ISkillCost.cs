using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillCost
{
    void SetCostValue(object o);
    bool CanPayCost(CharacterObject o, Vector4 point);
   bool PayCost(CharacterObject c, Vector4 point);
}
