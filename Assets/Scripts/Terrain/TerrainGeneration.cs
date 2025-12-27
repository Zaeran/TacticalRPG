using UnityEngine;
using System.Collections;

public class TerrainGeneration : MonoBehaviour {

    public GameObject terrainBlock;
	
	// Use this for initialization
	void Start () {
		for(int xPos = 0; xPos < 10; xPos++){
			for(int zPos = 0; zPos < 10; zPos++){
                GameObject block = Instantiate(terrainBlock, new Vector3(xPos, -2, zPos), Quaternion.identity);
                if ((xPos == 7 && zPos == 3) || (xPos == 1 && zPos == 1) || (xPos == 4 && zPos == 7))
                {
                    block.transform.localScale = new Vector3(1, 4, 1);
                }
                else
                {
                    block.transform.localScale = new Vector3(1, (float)((int)Random.Range(7, 10)) / 2, 1);
                }
            }
		}
	}
}
