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
	
	int damageReduction = 0;
	int evadeChance = 0;
	
	//longAction stuff
	bool longAction = false;
	int longActionAP = 0;
	int longActionPerformed = 0;
	GameObject longActionTarget = null;
	
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
	
	
	//key assignments
	const KeyCode moveButton = KeyCode.M;
	const KeyCode meleeButton = KeyCode.J;
	const KeyCode rangedButton = KeyCode.K;
	const KeyCode passButton = KeyCode.N;
	const KeyCode cancelButton = KeyCode.F;
	const KeyCode spellButton = KeyCode.L;
	
	void Awake () {
		//initialize all component variables
		findValid = GetComponent<FindValidPoints>();
		pathFind = GetComponent<PathfindingScript>();
		Draw = GetComponent<DrawSquaresScript>();
		Stats = GetComponent<AttributesScript>();
		Move = GetComponent<MovementScript>();
		Controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<TurnController>();
		Magic = GameObject.FindGameObjectWithTag("Controller").GetComponent<MagicList>();
		Weapons = GameObject.FindGameObjectWithTag("Controller").GetComponent<WeaponList>();
		Skills = GameObject.FindGameObjectWithTag("Controller").GetComponent<SkillList>();
		KnownAbilities = GetComponent<CharacterKnownAbilities>();
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
			//cancel action
			if(Input.GetKeyDown(cancelButton)){
			    ActionComplete();
			}

			if(Input.GetKeyDown(KeyCode.LeftShift)){
				Time.timeScale = 0.2f;
			}
			if(Input.GetKeyUp(KeyCode.LeftShift)){
				Time.timeScale = 1.0f;
			}
			
		    //end turn when AP is 0
			if(remainingAP == 0){
				isMyTurn = false;
				Controller.TurnOver();
			}
		}
	}

	//called by other objects when damage is inflicted
	public void TakeDamage(int damage){
		int damageTaken = damage - damageReduction;
		if(damageTaken < 0){
			damageTaken = 0;
		}
		if(Stats.Damage(damageTaken)){
			Controller.DeadCharacter(gameObject);
			Destroy(gameObject);
		}
		if(isReacting){
			ReactionComplete();
		}
		Time.timeScale = 1.0f;
	}

	public void LoseAP(int amount){
		remainingAP -= amount;
	}
	
	//movement is over. remove AP and reset everything
	public void StopMovingConfirmation(){
		if(!isReacting){
			remainingAP -= (movePath.Length - 1);
		}
		else{
			remainingAP -= (movePath.Length - 1) * 2;
			ReactionComplete();
		}
		ActionComplete();
	}
	
	//an action has been performed. allow new action to occur and remove all marker squares
	public void ActionComplete(int apCost = 0){
		remainingAP -= apCost;
		skillSelected = "";
		actionOccuring = false;
		isReacting = false;
		Draw.DestroyValidSquares();
		if(isReacting){
			ReactionComplete();
		}
	}
	//called when turn starts
	public void nextTurn(){
		isMyTurn = true;
		remainingAP = Stats.maxActions;
		reactionNo = 0;

		if(longAction){
			StartCoroutine(MagicAttack(longActionTarget));
		}
	}
	//enable reaction
	public void Reaction(GameObject target){
		reactionNo++;
		attackedFromTarget = target;
		isReacting = true;
	}
	
	private void ReactionComplete(){
		ActionComplete();
		isReacting = false;
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
		Collider[] col;
		switch(skillSelected){
		case "Move": //movement
			Skills.PerformSkill("Move", clickPosition, gameObject, wpnName);
			movePath = pathFind.StartPathFinding(clickPosition4D, validPoints,Stats.maxJump);
			if(movePath.Length > 0){
				Draw.DestroyValidSquares();
				Move.MoveToPoint(movePath, remainingAP);
			}
			actionOccuring = true;
			break;
		case "Attack": //melee attack
			Draw.DestroyValidSquares();
			if(KnownAbilities.SkillSucceeds("Attack", wpnName)){
				Skills.PerformSkill("Attack", clickPosition, gameObject, wpnName);
			}
			else{
				ActionComplete(3);
				Debug.Log("Skill Failed");
			}
			break;
		case "Magic":
			col = Physics.OverlapSphere(clickPosition, 0.3f);
			foreach(Collider c in col){
				targetObject = c.gameObject;
				StartCoroutine(MagicAttack(c.gameObject));
				break;
			}
			break;
		default:
			break;
		}
	}
	
	#region Actions
	
	void MoveAction(){
		skillSelected = "Move";
		validPoints = findValid.GetPoints("Move",remainingAP,Stats.maxJump);
		Draw.DrawValidSquares(validPoints);
	}

	void Attack(){
		skillSelected = "Attack";
		if(remainingAP >= Skills.getSkillCost(skillSelected)){
			int wpnType = Weapons.GetWeaponCombatStats(wpnName)[0];
			int wpnRange = Weapons.GetWeaponCombatStats(wpnName)[2];
			if(wpnType == 1 || wpnType == 2){
				validPoints = findValid.GetPoints("Melee", wpnRange,Stats.maxJump);
			}
			else{
				validPoints = findValid.GetPoints("Ranged", wpnRange,Stats.maxJump);
			}
			Draw.DrawValidSquares(validPoints);
		}
	}
	
	void MagicAction(){
		validPoints = findValid.GetPoints("Magic", 10,1);
		Draw.DrawValidSquares(validPoints);
		currentSpell = "DestroyBlock";
	}	
	#endregion
	
	#region Reactions
	//these will be added to SkillList
	void BlockReaction(){
		if(remainingAP > 3){
			actionOccuring = true;
			StartCoroutine(BlockRoutine());
		}
		else{
			ReactionComplete();
		}
	}
	void Dodge(){
		if(remainingAP > 2){
			skillSelected = "Move";
			validPoints = findValid.GetPoints("Move",1,Stats.maxJump);
			Draw.DrawValidSquares(validPoints);
		}
		else{
			ReactionComplete();
		}
	}
	void Manoeuvre(){
		
	}
	void CounterAttack(){
		
	}
	void Evade(){
		
	}
	#endregion
	
	#region ActionCoroutines
	
	IEnumerator MagicAttack(GameObject target){
		yield return new WaitForSeconds(1); //animation delay
		if(!longAction){
			longActionTarget = target;
			longAction = true;
			longActionAP = spellList["DestroyBlock"];
			longActionPerformed = 1;
		}
		int tempLongAP = longActionAP;
		longActionAP -= remainingAP;
		remainingAP -= tempLongAP;		
		if(longActionAP <= 0){
			longAction = false;
			longActionAP = 0;
			longActionTarget = null;
			longActionPerformed = 0;
			MethodInfo spell = Magic.GetType().GetMethod("Teleport");
			Debug.Log(spell.Name);
			spell.Invoke(Magic, new object[]{target, clickPosition});
		}
		ActionComplete();
		if(remainingAP < 0){
			remainingAP = 0;
		}
	}
	#endregion
	
	#region ReactionCoroutines
	IEnumerator BlockRoutine(){
		int blockValue = 2; //replace with shield/wpn value
		yield return new WaitForSeconds(0.5f); //block animation
		damageReduction += blockValue;
		ReactionComplete();
		yield return new WaitForSeconds(1.5f);
		damageReduction -= blockValue;
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
						Attack();
					}
					
					if(GUI.Button(new Rect(0,100,100,40), "MAGIC")){
						MagicAction();
					}
					
					if(GUI.Button(new Rect(0,200,100,40), "PASS")){
						ActionComplete();
						isMyTurn = false;
						if(!isReacting){
							Controller.TurnOver();
						}
					}
				}
				if(GUI.Button(new Rect(0,150,100,40), "CANCEL")){
					ActionComplete();
					if(isReacting){
						
					}
				}
				GUI.Box(new Rect(Screen.width - 80, 0, 80, 40), "AP: " + remainingAP);
			}
			else if(isReacting){
				if(GUI.Button(new Rect(0,0,100,40), "DODGE")){
					Dodge();
				}
				
				if(GUI.Button(new Rect(0,50,100,40), "BLOCK")){
					BlockReaction();
				}
			}
		}
	}
}
