using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnController : MonoBehaviour {
	
	List<GameObject> characters = new List<GameObject>();
	GameObject[] orderedCharacters = new GameObject[0];
	int currentTurn = 0;
	
	AttributesScript Stats;
	PlayerControlsScript charScript;
	AIScript aiScript;
	
	void Start(){
		StartBattle();
	}
	
	void StartBattle(){
		//add all characters in the battle to the character list
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
			characters.Add(g);
		}
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("NPC")){
			characters.Add(g);
		}

		int orderNumber = 0;
		orderedCharacters = new GameObject[characters.Count];
		
		//order characters in order of speed stat
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
			
		
	}
	
	public void TurnOver(){
		currentTurn++;
		if(currentTurn == orderedCharacters.Length){
			currentTurn = 0;
		}
		StartCoroutine(NextTurn());
	}
}
