using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectUnarmedDamage : ISkillEffect
{

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        foreach (ClickableTarget clickable in hitCharacters)
        {
            if (clickable is CharacterObject)
            {
                CharacterObject hitCharacter = clickable as CharacterObject;
                int totalDamage = hitCharacter.MyCharacter.ApplyDamage(1);
                FloatingNumbersScript.CreateFloatingNumber(hitCharacter.transform.position, totalDamage.ToString(), Color.red);
            }
        }
    }

        public string Description(CharacterObject c)
    {
        return string.Format("Deal 1 damage to targets");
    }
}
