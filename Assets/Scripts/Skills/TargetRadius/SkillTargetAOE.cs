using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetAOE : ISkillTargetRadius
{
    int aoeDistance = 1;

    public void SetAOE(int distance)
    {
        aoeDistance = distance;
    }

   public List<CharacterObject> GetTargets(CharacterObject c, Vector4 point)
    {
        List<Vector4> points = FindValidPoints.GetPoints("Ranged", point, aoeDistance, 1);

        List<CharacterObject> hitCharacters = new List<CharacterObject>();
        foreach (Vector4 square in points)
        {
            //Ensure a valid target
            RaycastHit hit;
            if (!Physics.Raycast(new Vector3(square.x, square.y + 1, square.z), Vector3.down, out hit))
            {
                continue;
            }
            CharacterObject hitCharacter = hit.collider.GetComponent<CharacterObject>();
            if (hitCharacter != null)
            {
                hitCharacters.Add(hitCharacter);
            }
        }

        if(hitCharacters.Count == 0)
        {
            return null;
        }
        return hitCharacters;
    }
}
