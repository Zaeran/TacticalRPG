using UnityEngine;
using System.Collections;

public class StatusSleep : MonoBehaviour {

	int duration = 0;
	CharacterStatus Status;
	
	void Start(){
		Status = GetComponent<CharacterStatus>();
		Status.sleep = true;
	}
	
	public void AddDuration(int d){
		duration += d;
	}
	
	public void NextTurn(){
		duration--;
		if(duration <= 0){
			Status.sleep = false;
			Destroy(this);
		}
	}
}
