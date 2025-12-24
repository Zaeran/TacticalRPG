using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillEffect
{
    void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point);
}
