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
	
	//components
	FindValidPoints findValid;
	PathfindingScript pathFind;
	PlayerDrawScript Draw;
	AttributesScript Stats;
	MovementScript Move;
	TurnController Controller;	
	AimProjectileScript ProjectileAim;
	ProjectileScript Projectile;
	
	//key assignments
	const KeyCode moveButton = KeyCode.M;
	const KeyCode meleeButton = KeyCode.J;
	const KeyCode rangedButton = KeyCode.K;
	const KeyCode passButton = KeyCode.N;
	const KeyCode cancelButton = KeyCode.F;
	
	void Awake () {
		//initialize all component variables
		findValid = GetComponent<FindValidPoints>();
		pathFind = GetComponent<PathfindingScript>();
		Draw = GetComponent<PlayerDrawScript>();
		Stats = GetComponent<AttributesScript>();
		Move = GetComponent<MovementScript>();
		ProjectileAim = GetComponent<AimProjectileScript>();
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
			//no option selected, so check for inputs
			if(optionType == 0){
				GetInputs();				
			}
			//cancel action
			if(Input.GetKeyDown(cancelButton)){
			    ActionComplete();
			}
			
			
			if(remainingAP == 0){
				isMyTurn = false;
				Controller.TurnOver();
			}
		}
	}
	
	//called by other objects when damage is inflicted
	public void TakeDamage(int damage){
		if(Stats.Damage(damage)){
			Controller.DeadCharacter(gameObject);
			Destroy(gameObject);
		}
	}
	
	//movement is over. remove AP and reset everything
	public void StopMovingConfirmation(){
		remainingAP -= (movePath.Length - 1);
		ActionComplete();
	}
	
	//an action has been performed. allow new action to occur and remove all marker squares
	private void ActionComplete(){
		optionType = 0;
		actionOccuring = false;
		Draw.DestroyValidSquares();
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
		if(Input.GetKeyDown(moveButton)){
			optionType = 1;	
			validPoints = findValid.GetPoints(optionType,remainingAP,Stats.maxJump);
			Draw.DrawValidSquares(validPoints);
		}
		
		if(Input.GetKeyDown(meleeButton)){
			if(remainingAP >= 3){
				optionType = 2;
				validPoints = findValid.GetPoints(optionType, 1,Stats.maxJump);
				Draw.DrawValidSquares(validPoints);
			}
		}
		
		if(Input.GetKeyDown(rangedButton)){
			if(remainingAP >= 3){
				optionType = 3;
				validPoints = findValid.GetPoints(optionType, 4, 2);
				Draw.DrawValidSquares(validPoints);
			}
		}
		
		if(Input.GetKeyDown(passButton)){
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
						break;
					}
				}
				if(clickPosition4D != Vector4.zero){ //the position clicked was a valid position
					LeftClickOptions();
				}
			}
		}
	}
	//possibilities of left-click
	private void LeftClickOptions(){
		Collider[] col;
		switch(optionType){
			case 1: //movement
				movePath = pathFind.StartPathFinding(clickPosition4D, validPoints,Stats.maxJump);
				if(movePath.Length > 0){
					Draw.DestroyValidSquares();
					Move.MoveToPoint(movePath, remainingAP);
				}
				actionOccuring = true;
				break;
			case 2: //melee attack
				col = Physics.OverlapSphere(clickPosition, 0.3f, ~groundOnlyLayer);
				foreach(Collider c in col){
					if(c.tag == "NPC"){
						StartCoroutine(MeleeAttack(c.gameObject));
						break;
					}
				}
				break;
			case 3: //ranged attack
				StartCoroutine(RangedAttack());
				break;
			default:
				break;
		}
	}
	
	//melee attack coroutine
	IEnumerator MeleeAttack(GameObject target){
		yield return new WaitForSeconds(1); //replace with animation
		target.SendMessage("TakeDamage", 1);
		remainingAP -= 3;
		ActionComplete();
	}
	
	//ranged attack coroutine
	IEnumerator RangedAttack(){
		yield return new WaitForSeconds(1); //replace with animation
		//create 'arrow', and fire it at the selected square
		GameObject cheese = Instantiate(Resources.Load("Objects/Arrow"), transform.position, Quaternion.identity) as GameObject;
		Projectile = cheese.GetComponent<ProjectileScript>();
		Projectile.Initialise(ProjectileAim.Aim(clickPosition), clickPosition);
		
		yield return new WaitForSeconds(1); //allow time for arrow to hit
		remainingAP -= 3;
		ActionComplete();
	}
}
