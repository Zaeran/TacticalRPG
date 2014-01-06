using UnityEngine;
using System.Collections;

public class StatusPoison : MonoBehaviour {

	int duration = 0;
	int damage = 0;
	CharacterStatus Status;
	
	void Start(){
		Status = GetComponent<CharacterStatus>();
		Status.poison = true;
	}
	
	public void AddDuration(int d){
		duration += d;
	}
	
	public void NextTurn(){
		duration--;
		gameObject.SendMessage("TakeDamage", 1);
		if(duration <= 0){
			Status.poison = false;
			Destroy(this);
		}
	}
}
