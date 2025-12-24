using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetSingle : ISkillTargetRadius
{
   public List<CharacterObject> GetTargets(CharacterObject c, Vector4 point)
    {
        List<CharacterObject> hitCharacters = new List<CharacterObject>();
        //Ensure a valid target
        RaycastHit hit;
        if (!Physics.Raycast(new Vector3(point.x, point.y + 1, point.z), Vector3.down, out hit))
        {
            return new List<CharacterObject>();
        }
        CharacterObject hitCharacter = hit.collider.GetComponent<CharacterObject>();
        hitCharacters.Add(hitCharacter);
        return hitCharacters;
    }
}
