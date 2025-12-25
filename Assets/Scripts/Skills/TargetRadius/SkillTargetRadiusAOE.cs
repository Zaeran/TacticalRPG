using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetRadiusAOE : ISkillTargetRadius
{
    int aoeDistance = 1;

    public SkillTargetRadiusAOE(int distance)
    {
        aoeDistance = distance;
    }

    public void SetAOE(int distance)
    {
        aoeDistance = distance;
    }

   public List<ClickableTarget> GetTargets(CharacterObject c, Vector4 point)
    {
        List<Vector4> points = FindValidPoints.GetPoints("Ranged", point, aoeDistance, 1);
        List<ClickableTarget> hitCharacters = new List<ClickableTarget>();
        foreach (Vector4 square in points)
        {
            //Ensure a valid target
            RaycastHit[] hits = Physics.RaycastAll(new Vector3(square.x, square.y + 1, square.z), Vector3.down);
            if(hits.Length == 0)
            {
                continue;
            }
            for(int i = 0; i < hits.Length; i++)
            {
                ClickableTarget hitObject = hits[i].collider.GetComponent<ClickableTarget>();
                if (hitObject != null)
                {
                    hitCharacters.Add(hitObject);
                }
            }
            
        }

        if(hitCharacters.Count == 0)
        {
            return null;
        }
        return hitCharacters;
    }
}
