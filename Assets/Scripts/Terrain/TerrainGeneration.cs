using UnityEngine;
using System.Collections;

public class TerrainGeneration : MonoBehaviour {
	
	const string TerrainBlockFilePath = "Objects/Terrain/GroundBlock";
	const string TerrainBaseFilePath = "Objects/Terrain/GroundBase";
	
	// Use this for initialization
	void Start () {
		for(int xPos = 0; xPos < 10; xPos++){
			for(int zPos = 0; zPos < 10; zPos++){
				int rand = Random.Range(1,3);
                if (xPos == 8 && zPos == 7)
                {
                    Instantiate(Resources.Load(string.Format("{0}2", TerrainBlockFilePath, rand)), new Vector3(xPos, 1, zPos), Quaternion.identity);
                }
                else if (xPos == 1 && zPos == 1)
                {
                    Instantiate(Resources.Load(string.Format("{0}2", TerrainBlockFilePath, rand)), new Vector3(xPos, 1, zPos), Quaternion.identity);
                }
                else
                {
                    Instantiate(Resources.Load(string.Format("{0}{1}", TerrainBlockFilePath, rand)), new Vector3(xPos, 1, zPos), Quaternion.identity);
                }
            }
		}
	}
}
