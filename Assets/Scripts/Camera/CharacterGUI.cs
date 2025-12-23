using UnityEngine;
using System.Collections;

public class CharacterGUI : MonoBehaviour {
	GameObject currentCharacter;
	GameObject tempChar;
	AttributesScript Stats;
	GenericControlsScript Character;
	
	public void UpdateTurn(GameObject character){
		currentCharacter = character;
		Stats = currentCharacter.GetComponent<AttributesScript>();
		Character = currentCharacter.GetComponent<GenericControlsScript>();
	}
	void OnGUI(){
		//if(currentCharacter != null){
		//	if(!Status.actionOccuring){
		//		if(Status.isMyTurn && !Status.targetReactionOccuring){
		//			if(Character.skillSelected == ""){
		//				if(Status.immobilize == 0){
		//					if(GUI.Button(new Rect(0,0,100,40), "MOVE")){
		//						Character.MoveAction();
		//					}
		//				}
						
		//				if(GUI.Button(new Rect(0,50,100,40), "ATTACK")){
		//					Character.SelectAction("Attack");
		//					Character.Skill();
		//				}
						
		//				if(GUI.Button(new Rect(0,150,100,40), "PASS")){
		//					Character.Pass();
		//				}
		//			}
		//			if(GUI.Button(new Rect(0,100,100,40), "CANCEL")){
		//				Character.ActionComplete();
		//			}
		//			GUI.Box(new Rect(Screen.width - 80, 0, 80, 40), "HP: " + Stats.CurrentHealth + "/" + Stats.MaxHealth);
		//			GUI.Box(new Rect(Screen.width - 80, 50, 80, 40), "AP: " + Stats.remainingAP + "/" + Stats.maxActions);
		//		}
		//		else if(Status.isReacting){
		//			if(GUI.Button(new Rect(0,0,100,40), "DODGE")){
		//				Character.Dodge();
		//			}
					
		//			if(GUI.Button(new Rect(0,50,100,40), "BLOCK")){
		//				Character.SelectAction("Block");
		//				Character.Skill();
		//			}
		//		}
		//	}
		//}
	}
}
