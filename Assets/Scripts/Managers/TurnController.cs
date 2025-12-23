using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnController {
	
	static List<CharacterObject> characters = new List<CharacterObject>();
	static int currentCharacterTurn = 0;
	static int currentRound; //What is the current round of combat?

	//components
	static RotateCamera cameraScript;
	static CharacterGUI guiScript;

	public static event CharacterEvent OnNextCharacterTurn;
	public static event CharacterEvent OnCharacterTurnEnding;
	
	void Start(){
		StartBattle();
		currentRound = 0;
		cameraScript = GameObject.FindGameObjectWithTag("MainCamPoint").GetComponent<RotateCamera>();
		guiScript = GameObject.FindGameObjectWithTag("GUIData").GetComponent<CharacterGUI>();
	}

	static void StartBattle(){
		TriggerNextTurn();
	}

	static void TriggerNextTurn(){
		Debug.Log("Current turn: " + CurrentCharacterTurn.MyCharacter.CharacterName);
		//Set turn active
		CurrentCharacterTurn.StartMyTurn();
        if (OnNextCharacterTurn != null)
        {
            OnNextCharacterTurn(CurrentCharacterTurn.MyCharacter);
        }
        //	cameraScript.SetFollow(CurrentCharacterTurn.gameObject); //Tell camera to follow the character
        //guiScript.UpdateTurn(orderedCharacters[currentTurn]); //Tell UI that it's now controlling the correct character
    }

	public static void AddCharacter(CharacterObject c)
    {
		if(characters == null)
        {
			characters = new List<CharacterObject>();
        }
		characters.Add(c);
    }
	static void OrderCharacters(){
		//int orderNumber = 0;
		//orderedCharacters = new GameObject[characters.Count];
		//for(int i = 0; i < )
		////Figure out initiative later
		//GetAllCharacters();
	}

	public static CharacterObject CurrentCharacterTurn
    {
        get { return characters[currentCharacterTurn]; }
    }
	
	public static void TurnOver(){
		if(OnCharacterTurnEnding != null)
        {
            OnCharacterTurnEnding(CurrentCharacterTurn.MyCharacter);
        }
		currentCharacterTurn++;
		if(currentCharacterTurn == characters.Count){
			currentCharacterTurn = 0;
			currentRound++;
		}
		TriggerNextTurn();
	}

	//character has died - we won't be reaching this point for a while
	public static void DeadCharacter(GameObject g){
		//bool enemyExists = false;
		//bool playerExists = false;
		//GameObject currentCharTurn = orderedCharacters[currentTurn];
		//Debug.Log("Characters alive: " + characters.Count);
		//characters.Remove(g);
		//Debug.Log("Characters alive: " + characters.Count);
		//foreach(GameObject go in characters){
		//	if(go.tag == "NPC"){
		//		enemyExists = true;
		//		break;
		//	}
		//}
		//foreach(GameObject go in characters){
		//	if(go.tag == "Player"){
		//		playerExists = true;
		//		break;
		//	}
		//}
		//if(!enemyExists){
		//	battleOver = true;
		//	Debug.Log("YOU WIN!");
		//	orderedCharacters[currentTurn].SendMessage("BattleOver");
		//}
		//else if(!playerExists){
		//	battleOver = true;
		//	Debug.Log("YOU LOSE");
		//	orderedCharacters[currentTurn].SendMessage("BattleOver");
		//}
		//else{
		//	OrderCharacters();
		//	if(orderedCharacters[currentTurn] != currentCharTurn){
		//		currentTurn--;
		//	}
		//}
	}
}
