using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControlsScript : MonoBehaviour {
	//variables and such
	RaycastHit rayhit;
	int optionType = 0;
	int groundOnlyLayer = 1 << 8;
	int notMoveableLayer = 1 << 9;
	Vector3 clickPosition = Vector3.zero;
	Vector4 clickPosition4D = Vector4.zero;
		
	List<Vector4> validPoints = new List<Vector4>();
	Vector3[] movePath = new Vector3[0];
	
	FindValidPoints findValid;
	PathfindingScript pathFind;
	PlayerDrawScript Draw;
	AttributesScript Attribute;
	
	
	void Start () {
		findValid = GetComponent<FindValidPoints>();
		pathFind = GetComponent<PathfindingScript>();
		Draw = GetComponent<PlayerDrawScript>();
		Attribute = GetComponent<AttributesScript>();
	}
	
	
	void Update () {
		//left mouse click
		if(Input.GetMouseButtonDown(0)){
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayhit, 100, groundOnlyLayer)){ //click on ground
				if(rayhit.normal == new Vector3(0,1,0)){ //ensures that only flat ground can be clicked on
					clickPosition = new Vector3(Mathf.Floor(rayhit.point.x + 0.5f), Mathf.Floor(rayhit.point.y), Mathf.Floor(rayhit.point.z + 0.5f));
					for(int i = 0; i < 20; i++){
						if(validPoints.Contains(new Vector4(clickPosition.x, clickPosition.y, clickPosition.z, i))){
							clickPosition4D = new Vector4(clickPosition.x, clickPosition.y, clickPosition.z,i);
							break;
						}
					}
					if(optionType == 1){
						movePath = pathFind.StartPathFinding(clickPosition4D, validPoints, Attribute.maxJump);
						if(movePath.Length > 0){
							optionType = 0;
							Draw.DestroyValidSquares();
						}
						Debug.Log("END POINT: " + clickPosition);
						for(int k = 0; k < movePath.Length; k++){
							Debug.Log(movePath[k]);
						}
					}
				}
			}
		}
		if(optionType == 0){
			if(Input.GetKeyDown(KeyCode.M)){
				validPoints = findValid.GetPoints(1, Attribute.maxMove, Attribute.maxJump);
				Draw.DrawValidSquares(validPoints);
				optionType = 1;
			}
			
		}
		if(Input.GetKeyDown(KeyCode.N)){
			Draw.DestroyValidSquares();
			Vector4 test = new Vector4(1,2,3,5);
			Vector3 test2 = test;
			optionType = 0;
		}
	}
}
