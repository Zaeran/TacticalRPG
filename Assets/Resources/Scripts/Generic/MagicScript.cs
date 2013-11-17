using UnityEngine;
using System.Collections;

public class MagicScript : MonoBehaviour {
	
	public int apCost;
	
	public void DestroyBlock(GameObject obj, Vector3 pos){
		Destroy(obj);
	}
	
	public void Teleport(GameObject obj, Vector3 pos){
	    transform.position = pos;
	}
	
	
	
}
