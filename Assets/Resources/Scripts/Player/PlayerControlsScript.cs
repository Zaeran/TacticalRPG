using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class PlayerControlsScript : MonoBehaviour {
	//variables and such
	bool actionOccuring = false;
	bool isMyTurn = false;
	bool isReacting = false;
	bool targetReactionOccuring = false;
	GameObject attackedFromTarget;
	GameObject targetObject;
	RaycastHit rayhit;
	int groundOnlyLayer = 1 << 8;
	public int remainingAP = 0;
	int reactionNo = 0;

	string skillSelected;
	
	Vector3 clickPosition = Vector3.zero;
	Vector4 clickPosition4D = Vector4.zero;
			
	List<Vector4> validPoints = new List<Vector4>();
	Vector3[] movePath = new Vector3[0];
	
	//casting
	Dictionary<string, int> spellList = new Dictionary<string, int>();
	string currentSpell = null;
	
	//components
	FindValidPoints findValid;
	PathfindingScript pathFind;
	DrawSquaresScript Draw;
	AttributesScript Stats;
	MovementScript Move;
	TurnController Controller;	
	ProjectileScript Projectile;
	MagicList Magic;
	WeaponList Weapons;
	SkillList Skills;
	CharacterKnownAbilities KnownAbilities;
	
	//weapon
	TextAsset weaponData;
	public string wpnName;
	
	void Awake () {
		//initialize all component variables
		Stats = GetComponent<AttributesScript>();
		Move = GetComponent<MovementScript>();
		KnownAbilities = GetComponent<CharacterKnownAbilities>();

		pathFind = GameObject.FindGameObjectWithTag("Controller").GetComponent<PathfindingScript>();
		findValid = GameObject.FindGameObjectWithTag("Controller").GetComponent<FindValidPoints>();
		Draw = GameObject.FindGameObjectWithTag("Controller").GetComponent<DrawSquaresScript>();
		Controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<TurnController>();
		Magic = GameObject.FindGameObjectWithTag("Controller").GetComponent<MagicList>();
		Weapons = GameObject.FindGameObjectWithTag("Controller").GetComponent<WeaponList>();
		Skills = GameObject.FindGameObjectWithTag("Controller").GetComponent<SkillList>();

		skillSelected = "";
	}	
	
	void Update () {
		if((isMyTurn || isReacting) && !targetReactionOccuring){ //can only act on your turn or if reacting
			//left mouse click
			if(Input.GetMouseButtonDown(0)){
				if(!actionOccuring){
					LeftClick();
				}
			}

		    //end turn when AP is 0
			if(remainingAP == 0 && isMyTurn){
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
		ActionComplete();
		try{
			gameObject.SendMessage("ReactionComplete");
		}
		catch{}
		Time.timeScale = 1.0f;
	}
	
	//movement is over. remove AP and reset everything
	public void StopMovingConfirmation(){
		if(!isReacting){
			ActionComplete(movePath.Length - 1);
		}
		else{
			ActionComplete((movePath.Length - 1) * 2);
		}
	}
	
	//an action has been performed. allow new action to occur and remove all marker squares
	public void ActionComplete(int apCost = 0){
		remainingAP -= apCost;
		skillSelected = "";
		actionOccuring = false;
		isReacting = false;
		Draw.DestroyValidSquares();
	}
	//called when turn starts
	public void nextTurn(){
		isMyTurn = true;
		remainingAP = Stats.maxActions;
		reactionNo = 0;
	}
	//enable reaction
	public void Reaction(GameObject target){
		reactionNo++;
		attackedFromTarget = target;
		isReacting = true;
	}
	
	//no opposition left. disable character
	public void BattleOver(){
		Debug.Log("END BATTLE");
		ActionComplete();
		remainingAP = 0;
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
		if(skillSelected == "Move"){
			movePath = pathFind.StartPathFinding(clickPosition4D, validPoints,Stats.maxJump, gameObject);
			if(movePath.Length > 0){
				Draw.DestroyValidSquares();
				Move.MoveToPoint(movePath, remainingAP);
			}
			actionOccuring = true;
		}
		else if(skillSelected != "" && Skills.getIsSkillTargeted(skillSelected)){
			actionOccuring = true;
			Draw.DestroyValidSquares();
			if(KnownAbilities.SkillSucceeds(skillSelected, wpnName)){
				Skills.PerformSkill(skillSelected, clickPosition, gameObject, Skills.getSkillCost(skillSelected), wpnName);
			}
			else{
				ActionComplete(Skills.getSkillCost(skillSelected));
				Debug.Log("Skill Failed");
			}
		}
	}
	
	#region Actions
	
	void MoveAction(){
		skillSelected = "Move";
		validPoints = findValid.GetPoints("Move", gameObject,remainingAP,Stats.maxJump);
		Draw.DrawValidSquares(validPoints);
	}

	void Skill(){
		if(remainingAP >= Skills.getSkillCost(skillSelected)){
			if(Skills.getIsSkillTargeted(skillSelected)){
				int wpnType = Weapons.GetWeaponCombatStats(wpnName)[0];
				int wpnRange = Weapons.GetWeaponCombatStats(wpnName)[2];
				if(wpnType == 1 || wpnType == 2){
					validPoints = findValid.GetPoints("Melee", gameObject, wpnRange,Stats.maxJump);
				}
				else{
					validPoints = findValid.GetPoints("Ranged", gameObject, wpnRange,Stats.maxJump);
				}
				Draw.DrawValidSquares(validPoints);
			}
			else{
				actionOccuring = true;
				if(KnownAbilities.SkillSucceeds(skillSelected, wpnName)){
					Skills.PerformSkill(skillSelected, Vector3.zero, gameObject, Skills.getSkillCost(skillSelected), wpnName);
				}
				else{
					ActionComplete(Skills.getSkillCost(skillSelected));
					Debug.Log("Skill Failed");
				}
			}
		}
		else{
			ActionComplete();
		}
	}
	#endregion
	
	#region Reactions
	//these will be added to SkillList
	void Dodge(){
		if(remainingAP > 2){
			skillSelected = "Move";
			validPoints = findValid.GetPoints("Move", gameObject,1,Stats.maxJump);
			Draw.DrawValidSquares(validPoints);
		}
		else{
			ActionComplete();
		}
	}
	#endregion
	
	void OnGUI(){
		if(!actionOccuring){
			if(isMyTurn && !targetReactionOccuring){
				if(skillSelected == ""){
					if(GUI.Button(new Rect(0,0,100,40), "MOVE")){
						MoveAction();
					}
					
					if(GUI.Button(new Rect(0,50,100,40), "ATTACK")){
						skillSelected = "Attack";
						Skill ();
					}
					
					if(GUI.Button(new Rect(0,150,100,40), "PASS")){
						Draw.DestroyValidSquares();
						ActionComplete();
						isMyTurn = false;
						if(!isReacting){
							Controller.TurnOver();
						}
					}
				}
				if(GUI.Button(new Rect(0,100,100,40), "CANCEL")){
					ActionComplete();
				}
				GUI.Box(new Rect(Screen.width - 80, 0, 80, 40), "HP: " + Stats.CurrentHealth + "/" + Stats.MaxHealth);
				GUI.Box(new Rect(Screen.width - 80, 50, 80, 40), "AP: " + remainingAP + "/" + Stats.maxActions);
			}
			else if(isReacting){
				if(GUI.Button(new Rect(0,0,100,40), "DODGE")){
					Dodge();
				}
				
				if(GUI.Button(new Rect(0,50,100,40), "BLOCK")){
					skillSelected = "Block";
					Skill ();
				}
			}
		}
	}
}
