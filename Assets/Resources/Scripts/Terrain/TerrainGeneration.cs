using UnityEngine;
using System.Collections;

public class TerrainGeneration : MonoBehaviour {
	
	const string TerrainBlockFilePath = "Objects/Terrain/GroundBlock";
	const string TerrainBaseFilePath = "Objects/Terrain/GroundBase";
	
	// Use this for initialization
	void Start () {
		for(int xPos = 0; xPos < 10; xPos++){
			for(int zPos = 0; zPos < 10; zPos++){
				float rand = Random.Range(1,4);
				Instantiate(Resources.Load(string.Format("{0}{1}", TerrainBlockFilePath, rand)), new Vector3(xPos, rand / 2, zPos), Quaternion.identity);
			}
		}
	}
}
