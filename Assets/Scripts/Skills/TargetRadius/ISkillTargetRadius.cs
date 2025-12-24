using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillTargetRadius
{
		List<CharacterObject> GetTargets(CharacterObject c, Vector4 point);
}
