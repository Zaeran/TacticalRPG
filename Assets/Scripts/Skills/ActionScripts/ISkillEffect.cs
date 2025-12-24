using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillEffect
{
    void ProcessEffect(CharacterObject c, List<CharacterObject> hitCharacters, Vector4 point);
}
