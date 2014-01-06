using UnityEngine;
using System.Collections;

public class StatusStun : MonoBehaviour {

	int duration = 0;
	CharacterStatus Status;
	
	void Start(){
		Status = GetComponent<CharacterStatus>();
		Status.stun = true;
	}
	
	public void AddDuration(int d){
		duration += d;
	}
	
	public void NextTurn(){
		duration--;
		if(duration <= 0){
			Status.stun = false;
			Destroy(this);
		}
	}
}
