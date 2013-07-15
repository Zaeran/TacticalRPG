using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnController : MonoBehaviour {
	
	List<GameObject> characters = new List<GameObject>();
	GameObject[] orderedCharacters = new GameObject[0];
	int currentTurn = 0;
	bool battleOver = false;
	
	AttributesScript Stats;
	PlayerControlsScript charScript;
	AIScript aiScript;
	RotateCamera cameraScript;
	
	void Start(){
		StartBattle();
		cameraScript = GameObject.FindGameObjectWithTag("MainCamPoint").GetComponent<RotateCamera>();
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
		Debug.Log(orderedCharacters[currentTurn].name);
		if(orderedCharacters[currentTurn].tag == ("Player")){
			charScript = orderedCharacters[currentTurn].GetComponent<PlayerControlsScript>();
			charScript.nextTurn();
		}
		else if(orderedCharacters[currentTurn].tag == ("NPC")){
			aiScript = orderedCharacters[currentTurn].GetComponent<AIScript>();
			aiScript.NextTurn();
		}
		cameraScript.SetFollow(orderedCharacters[currentTurn]);
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
			int highestSpeed = 0;
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
			charScript.BattleOver();
		}
		else if(!playerExists){
			battleOver = true;
			Debug.Log("YOU LOSE");
			aiScript.BattleOver();
		}
		else{
			OrderCharacters();
			if(orderedCharacters[currentTurn] != currentCharTurn){
				currentTurn--;
			}
		}
	}
}
