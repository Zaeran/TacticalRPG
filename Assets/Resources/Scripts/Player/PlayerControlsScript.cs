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
	int optionType = 0;
	int groundOnlyLayer = 1 << 8;
	public int remainingAP = 0;
	int reactionNo = 0;
	
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
	PlayerDrawScript Draw;
	AttributesScript Stats;
	MovementScript Move;
	TurnController Controller;	
	ProjectileScript Projectile;
	MagicScript Magic;
	
	//weapon
	TextAsset weaponData;
	public string wpnName;
	int wpnDamage;
	int wpnRange;
	
	
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
		Draw = GetComponent<PlayerDrawScript>();
		Stats = GetComponent<AttributesScript>();
		Move = GetComponent<MovementScript>();
		Controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<TurnController>();
		Magic = GetComponent<MagicScript>();
		spellList.Add("DestroyBlock", 7);
		weaponData = Resources.Load("Data/OneHandMeleeWeapons") as TextAsset;	
		SetWeaponStats();
		Debug.Log("Damage: " + wpnDamage + " Range: " + wpnRange);
	}	
	
	void Update () {
		if((isMyTurn || isReacting) && !targetReactionOccuring){ //can only act on your turn or if reacting
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
			
		    //end turn when AP is 0
			if(remainingAP == 0){
				isMyTurn = false;
				Controller.TurnOver();
			}
		}
	}
	
	void SetWeaponStats(){
		//get each line individually
		string[] testString = weaponData.text.Split('\n');
		//remove spaces from name
		string wpnNameNoSpaces = "";
		for(int i = 0; i < wpnName.Length; i++){
			if(wpnName[i] != ' '){
				wpnNameNoSpaces += wpnName[i];
			}
			
		}
		//get the data from the file
		foreach(string wpnData in testString){
			if(wpnData.StartsWith(wpnNameNoSpaces)){
				string[] wpn = wpnData.Split(' ');
				wpnDamage = int.Parse(wpn[1]);
				wpnRange = int.Parse(wpn[2]);
				break;
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
	private void ActionComplete(){
		optionType = 0;
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
		if(remainingAP < 3){
			ReactionComplete();
		}
		else{
			isReacting = true;
			StartCoroutine(ReactionTimer(reactionNo));
			//Block();
		}
	}
	
	IEnumerator ReactionTimer(int reactionNumber){
		yield return new WaitForSeconds(4);
		if(isReacting && reactionNumber == reactionNo){
			ActionComplete();
		}
	}
	
	private void ReactionComplete(){
		ActionComplete();
		isReacting = false;
		attackedFromTarget.SendMessage("ContinueFromReaction");
	}
	//enemy has finished reaction
	public void ContinueFromReaction(){
		if(optionType == 2){ //melee
			StartCoroutine(MeleeAttackEnd());
			targetReactionOccuring = false;
		}
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
		
		if(Input.GetKeyDown(spellButton)){
			optionType = 4;
			validPoints = findValid.GetPoints(optionType, 10,1);
			Draw.DrawValidSquares(validPoints);
		}
		
		if(Input.GetKeyDown(passButton)){
			ActionComplete();
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
				if(c.tag == "NPC" || c.tag == "Player"){
					targetObject = c.gameObject;
					StartCoroutine(MeleeAttackStart());
					break;
				}
			}
			break;
		case 3: //ranged attack
			StartCoroutine(RangedAttack(clickPosition));
			break;
		case 4:
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
		optionType = 1;
		validPoints = findValid.GetPoints(optionType,remainingAP,Stats.maxJump);
		Draw.DrawValidSquares(validPoints);
	}
	
	void MeleeAction(){
		if(remainingAP >= 3){
			optionType = 2;
			validPoints = findValid.GetPoints(optionType, wpnRange,Stats.maxJump);
			Draw.DrawValidSquares(validPoints);
		}
	}
	
	void RangedAction(){
		if(remainingAP >= 3){
			optionType = 3;
			validPoints = findValid.GetPoints(optionType, wpnRange, 2);
			Draw.DrawValidSquares(validPoints);
		}
	}
	
	void MagicAction(){
		optionType = 4;
		validPoints = findValid.GetPoints(optionType, 10,1);
		Draw.DrawValidSquares(validPoints);
		currentSpell = "DestroyBlock";
	}	
	#endregion
	
	#region Reactions
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
			optionType = 1;
			validPoints = findValid.GetPoints(optionType,1,Stats.maxJump);
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
	//melee attack coroutines
	IEnumerator MeleeAttackStart(){
		yield return new WaitForSeconds(0.5f); //replace with animation
		targetObject.SendMessage("Reaction", gameObject);
		Draw.DestroyValidSquares();
		targetReactionOccuring = true;
	}

	IEnumerator MeleeAttackEnd(){
		yield return new WaitForSeconds(0.5f); //replace with animation
		targetObject.SendMessage("TakeDamage", wpnDamage);
		targetReactionOccuring = false;
		remainingAP -= 3;
		ActionComplete();
	}
	
	//ranged attack coroutine
	IEnumerator RangedAttack(Vector3 aimPosition){
		const float projectileHeight = 0.4f;
		yield return new WaitForSeconds(1); //replace with animation
		//create 'arrow', and fire it at the selected square
		GameObject proj = Instantiate(Resources.Load("Objects/Arrow"), transform.position + new Vector3(0,projectileHeight,0), Quaternion.identity) as GameObject;
		Projectile = proj.GetComponent<ProjectileScript>();
		Projectile.Initialise(60, aimPosition, projectileHeight, wpnDamage);
		
		yield return new WaitForSeconds(1); //allow time for arrow to hit
		remainingAP -= 3;
		ActionComplete();
	}
	
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
		if(isMyTurn && !targetReactionOccuring){
			if(optionType == 0){
				if(GUI.Button(new Rect(0,0,100,40), "MOVE")){
					MoveAction();
				}
				
				if(GUI.Button(new Rect(0,50,100,40), "MELEE")){
					MeleeAction();
				}
				
				if(GUI.Button(new Rect(0,100,100,40), "RANGED")){
					RangedAction();
				}
				
				if(GUI.Button(new Rect(0,150,100,40), "MAGIC")){
					MagicAction();
				}
				
				if(GUI.Button(new Rect(0,250,100,40), "PASS")){
					ActionComplete();
					isMyTurn = false;
					if(!isReacting){
						Controller.TurnOver();
					}
				}
			}
			if(GUI.Button(new Rect(0,200,100,40), "CANCEL")){
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
