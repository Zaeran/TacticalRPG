using UnityEngine;
using System.Collections;

public class StatusEnchanted : MonoBehaviour {

	int duration = 0;
	CharacterStatus Status;
	
	void Start(){
		Status = GetComponent<CharacterStatus>();
		Status.enchanted = true;
	}
	
	public void AddDuration(int d){
		duration += d;
	}
	
	public void NextTurn(){
		duration--;
		if(duration <= 0){
			Status.enchanted = false;
			Destroy(this);
		}
	}
}
