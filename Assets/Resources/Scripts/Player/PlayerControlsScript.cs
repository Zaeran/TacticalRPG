using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControlsScript : MonoBehaviour {
	//variables and such
	bool actionOccuring = false;
	bool isMyTurn = false;
	RaycastHit rayhit;
	int optionType = 0;
	int groundOnlyLayer = 1 << 8;
	int remainingAP = 0;
	
	Vector3 clickPosition = Vector3.zero;
	Vector4 clickPosition4D = Vector4.zero;
			
	List<Vector4> validPoints = new List<Vector4>();
	Vector3[] movePath = new Vector3[0];
	
	FindValidPoints findValid;
	PathfindingScript pathFind;
	PlayerDrawScript Draw;
	AttributesScript Stats;
	MovementScript Move;
	TurnController Controller;
	
	
	void Awake () {
		findValid = GetComponent<FindValidPoints>();
		pathFind = GetComponent<PathfindingScript>();
		Draw = GetComponent<PlayerDrawScript>();
		Stats = GetComponent<AttributesScript>();
		Move = GetComponent<MovementScript>();
		Controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<TurnController>();
	}
	
	
	void Update () {
		if(isMyTurn){ //can only act on your turn
			//left mouse click
			if(Input.GetMouseButtonDown(0)){
				if(!actionOccuring){
					if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayhit, 100, groundOnlyLayer)){ //click on ground
						if(rayhit.normal == new Vector3(0,1,0)){ //ensures that only flat ground can be clicked on
							clickPosition = new Vector3(Mathf.Floor(rayhit.point.x + 0.5f), Mathf.Floor(rayhit.point.y), Mathf.Floor(rayhit.point.z + 0.5f));
							for(int i = 0; i < 20; i++){
								if(validPoints.Contains(new Vector4(clickPosition.x, clickPosition.y, clickPosition.z, i))){
									clickPosition4D = new Vector4(clickPosition.x, clickPosition.y, clickPosition.z,i);
									break;
								}
							}
							switch(optionType){
							case 1:
								movePath = pathFind.StartPathFinding(clickPosition4D, validPoints,Stats.maxJump);
								if(movePath.Length > 0){
									Draw.DestroyValidSquares();
									Move.MoveToPoint(movePath, remainingAP);
									remainingAP -= (movePath.Length - 1);
								}
								actionOccuring = true;
								break;
							case 2:
								optionType = 0;
								Debug.Log("ATTACK!");
								Draw.DestroyValidSquares();
								break;
							default:
								break;
							}
						}
					}
				}
			}
			if(optionType == 0){
				if(Input.GetKeyDown(KeyCode.M)){
					optionType = 1;	
					validPoints = findValid.GetPoints(optionType,remainingAP,Stats.maxJump);
					Draw.DrawValidSquares(validPoints);
				}
				
				if(Input.GetKeyDown(KeyCode.J)){
					optionType = 2;
					validPoints = findValid.GetPoints(optionType, 1,Stats.maxJump);
					Draw.DrawValidSquares(validPoints);
				}
				
				
			}
			if(Input.GetKeyDown(KeyCode.N)){
				Draw.DestroyValidSquares();
				optionType = 0;
			}
			if(remainingAP == 0){
				isMyTurn = false;
				Controller.TurnOver();
			}
		}
	}
	
	public void StopMovingConfirmation(){
		optionType = 0;
		actionOccuring = false;
	}
	
	public void nextTurn(){
		isMyTurn = true;
		remainingAP = Stats.maxActions;
	}
}
