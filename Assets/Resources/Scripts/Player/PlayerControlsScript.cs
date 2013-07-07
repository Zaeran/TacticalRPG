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
					LeftClick();
				}
			}
			if(optionType == 0){
				GetInputs();				
			}
			
			if(remainingAP == 0){
				isMyTurn = false;
				Controller.TurnOver();
			}
		}
	}
	
	public void StopMovingConfirmation(){
		remainingAP -= (movePath.Length - 1);
		ActionComplete();
	}
	
	private void ActionComplete(){
		optionType = 0;
		actionOccuring = false;
	}
	//called when turn starts
	public void nextTurn(){
		isMyTurn = true;
		remainingAP = Stats.maxActions;

	}
	//no opposition left. disable character
	public void BattleOver(){
		Debug.Log("END BATTLE");
		ActionComplete();
		remainingAP = 0;
	}
	//keyboard inputs
	private void GetInputs(){
		if(Input.GetKeyDown(KeyCode.M)){
			optionType = 1;	
			validPoints = findValid.GetPoints(optionType,remainingAP,Stats.maxJump);
			Draw.DrawValidSquares(validPoints);
		}
		
		if(Input.GetKeyDown(KeyCode.J)){
			if(remainingAP >= 3){
				optionType = 2;
				validPoints = findValid.GetPoints(optionType, 1,Stats.maxJump);
				Draw.DrawValidSquares(validPoints);
			}
		}
		
		if(Input.GetKeyDown(KeyCode.N)){
			isMyTurn = false;
			Controller.TurnOver();
		}
		
	}
	//left mouse click
	private void LeftClick(){
		clickPosition4D = Vector4.zero;
		//gets the in-game position of mouse, ensures that only flat ground is clicked, then uses that value for input
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayhit, 100, groundOnlyLayer)){ //click on ground
			if(rayhit.normal == new Vector3(0,1,0)){ //ensures that only flat ground can be clicked on
				clickPosition = new Vector3(Mathf.Floor(rayhit.point.x + 0.5f), rayhit.point.y, Mathf.Floor(rayhit.point.z + 0.5f));
				for(int i = 0; i < 20; i++){
					if(validPoints.Contains(new Vector4(clickPosition.x, clickPosition.y, clickPosition.z, i))){
						clickPosition4D = new Vector4(clickPosition.x, clickPosition.y, clickPosition.z,i);
						Debug.Log(clickPosition4D);
						break;
					}
				}
				if(clickPosition4D != Vector4.zero){
					LeftClickOptions();
				}
			}
		}
	}
	//possibilities of left-click
	private void LeftClickOptions(){
		switch(optionType){
		case 1:
			movePath = pathFind.StartPathFinding(clickPosition4D, validPoints,Stats.maxJump);
			if(movePath.Length > 0){
				Draw.DestroyValidSquares();
				Move.MoveToPoint(movePath, remainingAP);
			}
			actionOccuring = true;
			break;
		case 2:
			Collider[] col = Physics.OverlapSphere(clickPosition, 0.3f, ~groundOnlyLayer);
			foreach(Collider c in col){
				if(c.tag == "NPC"){
					StartCoroutine(MeleeAttack(c.gameObject));
					break;
				}
			}
			break;
		default:
			break;
		}
	}
	
	IEnumerator MeleeAttack(GameObject target){
		yield return new WaitForSeconds(1);
		Destroy(target);
		Controller.DeadCharacter(target);
		Draw.DestroyValidSquares();
		remainingAP -= 3;
		ActionComplete();
		
	}
}
