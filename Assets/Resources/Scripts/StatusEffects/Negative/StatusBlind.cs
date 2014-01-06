using UnityEngine;
using System.Collections;

public class StatusBlind : MonoBehaviour {

	int duration = 0;
	CharacterStatus Status;

	void Start(){
		Status = GetComponent<CharacterStatus>();
		Status.blind = true;
	}

	public void AddDuration(int d){
		duration += d;
	}

	public void NextTurn(){
		duration--;
		if(duration <= 0){
			Status.blind = false;
			Destroy(this);
		}
	}
}
