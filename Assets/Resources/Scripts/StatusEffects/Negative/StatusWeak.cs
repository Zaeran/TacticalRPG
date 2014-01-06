using UnityEngine;
using System.Collections;

public class StatusWeak : MonoBehaviour {

	int duration = 0;
	CharacterStatus Status;
	
	void Start(){
		Status = GetComponent<CharacterStatus>();
		Status.weak = true;
	}
	
	public void AddDuration(int d){
		duration += d;
	}
	
	public void NextTurn(){
		duration--;
		if(duration <= 0){
			Status.weak = false;
			Destroy(this);
		}
	}
}
