using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GenericControlsScript : MonoBehaviour {
	//variables and such
	bool isPlayer = false;
	GameObject attackedFromTarget;
	GameObject targetObject;
	public int Stats.remainingAP = 0;
	int reactionNo = 0;

	public string skillSelected;

	//pathfind
	List<Vector4> validPoints = new List<Vector4>();
	Vector3[] movePath = new Vector3[0];
	
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
	CharacterStatus Status;
	MouseControlScript Mouse;
	
	//weapon
	TextAsset weaponData;
	public string wpnName;
	
	void Awake () {
		//initialize all component variables
		Stats = GetComponent<AttributesScript>();
		KnownAbilities = GetComponent<CharacterKnownAbilities>();
		Status = GetComponent<CharacterStatus>();

		Move = GameObject.FindGameObjectWithTag("Controller").GetComponent<MovementScript>();
		pathFind = GameObject.FindGameObjectWithTag("Controller").GetComponent<PathfindingScript>();
		findValid = GameObject.FindGameObjectWithTag("Controller").GetComponent<FindValidPoints>();
		Draw = GameObject.FindGameObjectWithTag("Controller").GetComponent<DrawSquaresScript>();
		Controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<TurnController>();
		Magic = GameObject.FindGameObjectWithTag("Controller").GetComponent<MagicList>();
		Weapons = GameObject.FindGameObjectWithTag("Controller").GetComponent<WeaponList>();
		Skills = GameObject.FindGameObjectWithTag("Controller").GetComponent<SkillList>();
		if(GetComponent<MouseControlScript>()){
			Mouse = GetComponent<MouseControlScript>();
			isPlayer = true;
		}

		skillSelected = "";
	}	
	
	void Update () {
		if((Status.isMyTurn || Status.isReacting) && !Status.targetReactionOccuring){ //can only act on your turn or if reacting
		    //end turn when AP is 0
			if(Stats.remainingAP == 0 && Status.isMyTurn){
				Status.isMyTurn = false;
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
	public void MovementOver(int distanceTravelled){
		if(!Status.isReacting){
			ActionComplete(distanceTravelled);
		}
		else{
			ActionComplete(distanceTravelled * 2);
		}
	}
	
	//an action has been performed. allow new action to occur and remove all marker squares
	public void ActionComplete(int apCost = 0){
		Stats.remainingAP -= apCost;
		skillSelected = "";
		Status.actionOccuring = false;
		Status.isReacting = false;
		Mouse.PositionSelected();
		Draw.DestroyValidSquares();
	}
	//called when turn starts
	public void nextTurn(){
		Status.isMyTurn = true;
		Stats.remainingAP = Stats.maxActions;
		reactionNo = 0;
	}
	//enable reaction
	public void Reaction(GameObject target){
		reactionNo++;
		attackedFromTarget = target;
		Status.isReacting = true;
	}
	
	//no opposition left. disable character
	public void BattleOver(){
		Debug.Log("END BATTLE");
		ActionComplete();
		Stats.remainingAP = 0;
	}
	
	#region Actions
	public void SelectAction(string skill){
		skillSelected = skill;
	}
	public void SelectPosition(Vector4 position){
		if(skillSelected == "Move"){
			movePath = pathFind.StartPathFinding(position, validPoints,Stats.maxJump, gameObject);
			if(movePath.Length > 0){
				Draw.DestroyValidSquares();
				Move.MoveToPoint(gameObject, movePath, Stats.remainingAP);
			}
			Status.actionOccuring = true;
		}
		else if(skillSelected != "" && Skills.getIsSkillTargeted(skillSelected)){
			Status.actionOccuring = true;
			Draw.DestroyValidSquares();
			if(KnownAbilities.SkillSucceeds(skillSelected, wpnName)){
				Skills.PerformSkill(skillSelected, position, gameObject, Skills.getSkillCost(skillSelected), wpnName);
			}
			else{
				ActionComplete(Skills.getSkillCost(skillSelected));
				Debug.Log("Skill Failed");
			}
		}
	}
	
	public void MoveAction(){
		skillSelected = "Move";
		validPoints = findValid.GetPoints("Move", gameObject,Stats.remainingAP,Stats.maxJump);
		Draw.DrawValidSquares(validPoints);
		Mouse.SelectPosition(validPoints);
	}

	public void Skill(){
		if(Stats.remainingAP >= Skills.getSkillCost(skillSelected)){
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
				if(isPlayer){
					Mouse.SelectPosition(validPoints);
				}

			}
			else{
				Status.actionOccuring = true;
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

	public void Pass(){
		Draw.DestroyValidSquares();
		ActionComplete();
		Status.isMyTurn = false;
		if(!Status.isReacting){
			Controller.TurnOver();
		}
	}
	#endregion
	
	#region Reactions
	//these will be added to SkillList
	public void Dodge(){
		if(Stats.remainingAP > 2){
			skillSelected = "Move";
			validPoints = findValid.GetPoints("Move", gameObject,1,Stats.maxJump);
			Draw.DrawValidSquares(validPoints);
		}
		else{
			ActionComplete();
		}
	}
	#endregion
}