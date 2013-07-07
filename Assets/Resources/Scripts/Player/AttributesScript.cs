using UnityEngine;
using System.Collections;

public class AttributesScript : MonoBehaviour {
	
	public int speedStat;
	public int maxJump;
	public int maxActions;
	
	//base stats
	public int MaxHealth;
	public int CurrentHealth;	
	
	private void Start(){
		CurrentHealth = MaxHealth;
	}
	
	public bool Damage(int damage){
		CurrentHealth -= damage;
		if(CurrentHealth <= 0){
			return true;
		}
		else{
			return false;
		}
	}
	
}
