using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillTargetRadius
{
		List<ClickableTarget> GetTargets(CharacterObject c, Vector4 point);
		string Description(CharacterObject c);
		bool RequiresConfirmation();
}
