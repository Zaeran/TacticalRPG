using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetRadiusAOE : ISkillTargetRadius
{
    int _aoeDistance = 1;
    bool _canHitTerrain = false;
    bool _canHitCharacters = false;

    public SkillTargetRadiusAOE(int distance, bool canHitTerrain, bool canHitCharacters)
    {
        _aoeDistance = distance;
        _canHitTerrain = canHitTerrain;
        _canHitCharacters = canHitCharacters;
    }

    public void SetAOE(int distance)
    {
        _aoeDistance = distance;
    }

   public List<ClickableTarget> GetTargets(CharacterObject c, Vector4 point)
    {
        List<Vector4> points = FindValidPoints.GetPoints("Ranged", point, _aoeDistance, 1, true);
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
                    if ((hitObject is TerrainObject && _canHitTerrain) || (hitObject is CharacterObject && _canHitCharacters))
                    {
                        hitCharacters.Add(hitObject);
                    }
                }
            }
            
        }

        if(hitCharacters.Count == 0)
        {
            return null;
        }
        return hitCharacters;
    }

     public string Description(CharacterObject c)
    {
        return _aoeDistance.ToString();
    }
}
