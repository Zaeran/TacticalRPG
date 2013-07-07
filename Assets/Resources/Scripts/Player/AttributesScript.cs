using UnityEngine;
using System.Collections;

public class AttributesScript : MonoBehaviour {
	
	public int speedStat;
	public int maxJump;
	public int maxActions;
	
	//base stats
	public int MaxHealth;
	public int CurrentHealth;	
	
	public bool TakeDamage(int damage){
		CurrentHealth -= damage;
		if(CurrentHealth <= 0){
			return true;
		}
		else{
			return false;
		}
	}
	
}
