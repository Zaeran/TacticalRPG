using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawSquaresScript : MonoBehaviour {

	public static DrawSquaresScript instance;

	public GameObject validSquareObject;
	static List<GameObject> validTargetSquares = new List<GameObject>();
	public GameObject aoeSquareObject;
	static List<GameObject> aoeTargetSquares = new List<GameObject>();


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
			validTargetSquares.Add(Instantiate(instance.validSquareObject, squarePosition + (Vector3.up * 0.02f), Quaternion.identity));

		}
	}

	public static void DrawAOETagetSquares(List<Vector4> squares)
    {
		DestroyAOESquares();
		foreach (Vector4 square in squares)
		{
			Vector3 squarePosition = square;
			aoeTargetSquares.Add(Instantiate(instance.aoeSquareObject, squarePosition + (Vector3.up * 0.04f), Quaternion.identity));

		}
	}
	
	//removes all created squares
	public static void DestroyValidSquares(){
		foreach(GameObject g in validTargetSquares){
			Destroy(g);
		}
		validTargetSquares = new List<GameObject>();
	}

	public static void DestroyAOESquares()
    {
		foreach(GameObject g in aoeTargetSquares)
        {
			Destroy(g);
        }
		aoeTargetSquares = new List<GameObject>();
    }
	
}
