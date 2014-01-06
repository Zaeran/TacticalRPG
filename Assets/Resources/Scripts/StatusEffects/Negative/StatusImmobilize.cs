using UnityEngine;
using System.Collections;

public class StatusImmobilize : MonoBehaviour {

	int duration = 0;
	CharacterStatus Status;
	
	void Start(){
		Status = GetComponent<CharacterStatus>();
		Status.immobilize = true;
	}
	
	public void AddDuration(int d){
		duration += d;
	}
	
	public void NextTurn(){
		duration--;
		if(duration <= 0){
			Status.immobilize = false;
			Destroy(this);
		}
	}
}
