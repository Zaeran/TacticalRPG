using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrereqStandingOnTerrainType : ISkillPrerequisite
{
    List<TerrainType> _requiredTerrainTypes;

    public SkillPrereqStandingOnTerrainType(List<TerrainType> terrainTypes)
    {
        _requiredTerrainTypes = terrainTypes;
    }

    public bool MeetsPrerequisite(CharacterObject c)
    {
        RaycastHit hit;
        if(Physics.Raycast(c.transform.position + new Vector3(0,0.01f,0), Vector3.down, out hit))
        {
            TerrainObject terrain = hit.collider.gameObject.GetComponent<TerrainObject>();
            if(terrain == null)
            {
                return false;
            }
            foreach(TerrainType t in _requiredTerrainTypes)
            {
                if(terrain.MyTerrainType == t)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public string GetPrerequisiteFailureText()
    {
        string terrainTypes = "";
        foreach (TerrainType t in _requiredTerrainTypes)
        {
            terrainTypes += t + ",";
        }
        return "Requires standing on: " + terrainTypes.Trim(',');
    }

    public string Description(CharacterObject c)
    {
        string terrainTypes = "";
        foreach(TerrainType t in _requiredTerrainTypes)
        {
            terrainTypes += t + ",";
        }
        return "Standing on terrain: " + terrainTypes.Trim(',');
    }
}
