using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDrawScript : MonoBehaviour {
	
	public GameObject Node;
	List<GameObject> createdObject = new List<GameObject>();
	
	public void DrawValidSquares(List<Vector4> squares){
		foreach(Vector4 square in squares){
			if(square.w != 0){
				Vector3 squarePosition = square;
				createdObject.Add(Instantiate(Node, squarePosition + (Vector3.up * 0.02f), Quaternion.identity) as GameObject);
			}
		}
	}
	
	public void DestroyValidSquares(){
		foreach(GameObject g in createdObject){
			Destroy(g);
		}
		createdObject = new List<GameObject>();
	}
	
}
