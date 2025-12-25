using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectHealing : ISkillEffect
{
    int _healing;

    public SkillEffectHealing(int healing)
    {
        _healing = healing;
    }

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        foreach (ClickableTarget clickable in hitCharacters)
        {
            if (clickable is CharacterObject)
            {
                CharacterObject hitCharacter = clickable as CharacterObject;
                int totalDamage = hitCharacter.MyCharacter.ApplyHealing(_healing);
                FloatingNumbersScript.CreateFloatingNumber(hitCharacter.transform.position, totalDamage.ToString(), Color.green);
            }
        }
    }
}
