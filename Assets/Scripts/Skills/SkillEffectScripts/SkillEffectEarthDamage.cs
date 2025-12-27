using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectEarthDamage : ISkillEffect
{
    int _damage;

    public SkillEffectEarthDamage(int damage)
    {
        _damage = damage;
    }

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        foreach (ClickableTarget clickable in hitCharacters)
        {
            if (clickable is CharacterObject)
            {
                CharacterObject hitCharacter = clickable as CharacterObject;
                int totalDamage = hitCharacter.MyCharacter.ApplyDamage(_damage);
                FloatingNumbersScript.CreateFloatingNumber(hitCharacter.transform.position, totalDamage.ToString(), Color.red);
            }
        }
    }

        public string Description(CharacterObject c)
    {
        return string.Format("Deal {0} damage to targets", _damage);
    }
}
