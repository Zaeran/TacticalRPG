using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class GenericControlsScript : MonoBehaviour {
	//variables and such
	bool isPlayer = false;
	GameObject attackedFromTarget;
	public int remainingAP = 0;
	int reactionNo = 0;

	public string skillSelected;

	//pathfind
	List<Vector4> validPoints = new List<Vector4>();
	Vector3[] movePath = new Vector3[0];
	
	//components
	DrawSquaresScript Draw;
	AttributesScript Stats;
	MovementScript Move;
	MouseControlScript Mouse;
	
	//weapon
	TextAsset weaponData;
	public string wpnName;

	public event VoidEevent OnClearSquares;

	//ADD: Magic
	void Awake () {
		//initialize all component variables
		Stats = GetComponent<AttributesScript>();

		Move = GameObject.FindGameObjectWithTag("Controller").GetComponent<MovementScript>();
		Draw = GameObject.FindGameObjectWithTag("Controller").GetComponent<DrawSquaresScript>();

		if(GetComponent<MouseControlScript>()){
			Mouse = GetComponent<MouseControlScript>();
			isPlayer = true;
		}

		skillSelected = "";
	}	

	//called by other objects when damage is inflicted
	public void TakeDamage(int damage){
		if(Stats.Damage(damage)){
			TurnController.DeadCharacter(gameObject);
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
	//	if(!Status.isReacting){
	//		ActionComplete(distanceTravelled);
	//	}
	//	else{
	//		ActionComplete(distanceTravelled * 2);
	//	}
	}
	
	//an action has been performed. allow new action to occur and remove all marker squares
	public void ActionComplete(int apCost = 0){
		//Stats.remainingAP -= apCost;
		//skillSelected = "";
		//Status.actionOccuring = false;
		//Status.isReacting = false;
		//Mouse.PositionSelected();
		//Draw.DestroyValidSquares();
	}
	//called when turn starts
	public void NextTurn(){
		//Status.isMyTurn = true;
		//Stats.remainingAP = Stats.maxActions;
		//reactionNo = 0;
	}
	//enable reaction
	public void Reaction(GameObject target){
		//GameObject.FindGameObjectWithTag("GUIData").SendMessage("UpdateTurn", gameObject);
		//reactionNo++;
		//attackedFromTarget = target;
		//Status.isReacting = true;
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
        //if (skillSelected == "Move")
        //{
        //    movePath = Pathfinding.StartPathFinding(position, validPoints, Stats.maxJump, gameObject);
        //    if (movePath.Length > 0)
        //    {
        //        Draw.DestroyValidSquares();
        //        Move.MoveToPoint(gameObject, movePath, Stats.remainingAP);
        //    }
        //    Status.actionOccuring = true;
        //}



        //else if (skillSelected != "" && Skills.getIsSkillTargeted(skillSelected))
        //{
        //    Status.actionOccuring = true;
        //    Draw.DestroyValidSquares();
        //    if (KnownAbilities.SkillSucceeds(skillSelected))
        //    {
        //        Skills.PerformSkill(skillSelected, position, gameObject, Skills.getSkillCost(skillSelected));
        //    }
        //    else
        //    {
        //        ActionComplete(Skills.getSkillCost(skillSelected));
        //        Debug.Log("Skill Failed");
        //    }
        //}
    }
	
	public void MoveAction(int distance = 0){
		skillSelected = "Move";
		if(distance == 0){ 
			distance = Stats.remainingAP;
		}
		validPoints = FindValidPoints.GetPoints("Move", gameObject,distance,Stats.maxJump);
		DrawSquaresScript.DrawValidSquares(validPoints);
		Mouse.SelectPosition(validPoints);
	}

	public void Skill(){
		//if(Stats.remainingAP >= Skills.getSkillCost(skillSelected)){
		//	if(Skills.getIsSkillTargeted(skillSelected)){
		//		if(Skills.GetSkillRange(skillSelected) == "Weapon"){
		//			string wpnType1 = "";
		//			int wpnRange1 = 0;
		//			string wpnType2 = "";
		//			int wpnRange2 = 0;
		//			string[] allEquipment = Equipment.GetAllEquipment();
		//			if(allEquipment[0] != ""){ //ADD: Proper support for dual weapon wielding
		//				if(Items.GetItemType(allEquipment[0]) == "Weapon"){
		//					wpnType1 = Items.GetWpnType(allEquipment[0]);
		//					wpnRange1 = Items.GetWpnRange(allEquipment[0]);
		//				}
		//			}
		//			else{
		//				wpnType1 = "Melee";
		//				wpnRange1 = 1;
		//			}
		//			if(allEquipment[1] != ""){
		//				if(Items.GetItemType(allEquipment[1]) == "Weapon"){
		//					wpnType2 = Items.GetWpnType(allEquipment[1]);
		//					wpnRange2 = Items.GetWpnRange(allEquipment[1]);
		//				}
		//			}
		//			else{
		//				wpnType2 = "Melee";
		//				wpnRange2 = 0;
		//			}

		//			validPoints = FindValidPoints.instance.GetPoints(wpnType1, gameObject, wpnRange1,Stats.maxJump);
		//		}
		//		else{
		//			validPoints = FindValidPoints.instance.GetPoints("Magic", gameObject, int.Parse(Skills.GetSkillRange(skillSelected)));
		//		}
		//		Draw.DrawValidSquares(validPoints);
		//		if(isPlayer){
		//			Mouse.SelectPosition(validPoints);
		//		}

		//	}
		//	else{
		//		Status.actionOccuring = true;
		//		if(KnownAbilities.SkillSucceeds(skillSelected)){
		//			Skills.PerformSkill(skillSelected, Vector3.zero, gameObject, Skills.getSkillCost(skillSelected)); //ADD: Skil cost modfiers
		//		}
		//		else{
		//			ActionComplete(Skills.getSkillCost(skillSelected));
		//			Debug.Log("Skill Failed");
		//		}
		//	}
		//}
		//else{
		//	ActionComplete();
		//}
	}

	public void Pass(){
		//Draw.DestroyValidSquares();
		//ActionComplete();
		//Status.isMyTurn = false;
		//if(!Status.isReacting){
		//	Controller.TurnOver();
		//}
	}
	#endregion
	
	#region Reactions
	//these will be added to SkillList
	public void Dodge(){
		if(Stats.remainingAP > 2){
			MoveAction(1);
		}
		else{
			ActionComplete();
		}
	}
	#endregion
}
