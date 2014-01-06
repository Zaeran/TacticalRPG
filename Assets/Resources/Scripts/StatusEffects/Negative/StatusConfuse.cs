using UnityEngine;
using System.Collections;

public class StatusConfuse : MonoBehaviour {

	int duration = 0;
	CharacterStatus Status;
	
	void Start(){
		Status = GetComponent<CharacterStatus>();
		Status.confuse = true;
	}
	
	public void AddDuration(int d){
		duration += d;
	}
	
	public void NextTurn(){
		duration--;
		if(duration <= 0){
			Status.confuse = false;
			Destroy(this);
		}
	}
}
