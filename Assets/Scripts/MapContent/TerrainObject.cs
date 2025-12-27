using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainObject : ClickableTarget
{
    TerrainType _myTerrainType;

    public void Start()
    {
        if ((int)Random.Range(0, 5) == 2)
        {
            _myTerrainType = TerrainType.Water;
        }
        else
        {
            _myTerrainType = TerrainType.Rock;
        }
        
        GetComponent<MeshRenderer>().material.color = _myTerrainType == TerrainType.Rock ? Color.gray : Color.blue;
    }

    public TerrainType MyTerrainType
    {
        get { return _myTerrainType; }
    }
}
