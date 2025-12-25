using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawSquaresScript : MonoBehaviour {

	public static DrawSquaresScript instance;

	public GameObject Node;
	static List<GameObject> createdObject = new List<GameObject>();

    private void Awake()
    {
		instance = this;
    }

	//draws sqares at the locations given in the list
	public static void DrawValidSquares(List<Vector4> squares)
	{
		DestroyValidSquares();
		foreach (Vector4 square in squares)
		{
			Vector3 squarePosition = square;
			createdObject.Add(Instantiate(instance.Node, squarePosition + (Vector3.up * 0.02f), Quaternion.identity) as GameObject);

		}
	}
	
	//removes all created squares
	public static void DestroyValidSquares(){
		foreach(GameObject g in createdObject){
			Destroy(g);
		}
		createdObject = new List<GameObject>();
	}
	
}
