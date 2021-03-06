using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnController : MonoBehaviour {
	
	List<GameObject> characters = new List<GameObject>();
	GameObject[] orderedCharacters = new GameObject[0];
	int currentTurn = 0;
	int currentRound;
	bool battleOver = false;

	//components
	AttributesScript Stats;
	RotateCamera cameraScript;
	CharacterGUI guiScript;
	
	void Start(){
		StartBattle();
		currentRound = 0;
		cameraScript = GameObject.FindGameObjectWithTag("MainCamPoint").GetComponent<RotateCamera>();
		guiScript = GameObject.FindGameObjectWithTag("GUIData").GetComponent<CharacterGUI>();
	}
	
	void StartBattle(){
		//add all characters in the battle to the character list
		GetAllCharacters();
		//order characters in order of speed stat
		OrderCharacters();

		
		StartCoroutine(NextTurn());
	}
	
	IEnumerator NextTurn(){
		yield return new WaitForSeconds(1);
		Debug.Log(currentRound + ": " + orderedCharacters[currentTurn].name);
		orderedCharacters[currentTurn].SendMessage("NextTurn");
		cameraScript.SetFollow(orderedCharacters[currentTurn]);
		guiScript.UpdateTurn(orderedCharacters[currentTurn]);
	}
	void GetAllCharacters(){
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
			characters.Add(g);
		}
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("NPC")){
			characters.Add(g);
		}
	}
	
	void OrderCharacters(){
		int orderNumber = 0;
		Debug.Log("ALIVE CHARS: " + characters.Count);
		orderedCharacters = new GameObject[characters.Count];
		while(characters.Count != 0){
			int highestSpeed = -1;
			GameObject highestSpeedObject = null;
			
			foreach(GameObject g in characters){
				Stats = g.GetComponent<AttributesScript>();
				if(Stats.speedStat > highestSpeed){
					highestSpeed = Stats.speedStat;
					highestSpeedObject = g;
				}
			}
			orderedCharacters[orderNumber] = highestSpeedObject;
			characters.Remove(highestSpeedObject);
			orderNumber++;
		}
		GetAllCharacters();
	}
	
	public void TurnOver(){
		currentTurn++;
		if(currentTurn == orderedCharacters.Length){
			currentTurn = 0;
			currentRound++;
		}
		if(!battleOver){
			StartCoroutine(NextTurn());
		}
	}
	
	//character has died
	public void DeadCharacter(GameObject g){
		bool enemyExists = false;
		bool playerExists = false;
		GameObject currentCharTurn = orderedCharacters[currentTurn];
		Debug.Log("Characters alive: " + characters.Count);
		characters.Remove(g);
		Debug.Log("Characters alive: " + characters.Count);
		foreach(GameObject go in characters){
			if(go.tag == "NPC"){
				enemyExists = true;
				break;
			}
		}
		foreach(GameObject go in characters){
			if(go.tag == "Player"){
				playerExists = true;
				break;
			}
		}
		if(!enemyExists){
			battleOver = true;
			Debug.Log("YOU WIN!");
			orderedCharacters[currentTurn].SendMessage("BattleOver");
		}
		else if(!playerExists){
			battleOver = true;
			Debug.Log("YOU LOSE");
			orderedCharacters[currentTurn].SendMessage("BattleOver");
		}
		else{
			OrderCharacters();
			if(orderedCharacters[currentTurn] != currentCharTurn){
				currentTurn--;
			}
		}
	}
}
