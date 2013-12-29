using UnityEngine;
using System.Collections;

public class AttributesScript : MonoBehaviour {

	CharacterBuffsDebuffs statusEffects;

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
		int damageTaken = Mathf.Clamp(damage - statusEffects.damageReductionPhysical, 0, 10000);
		CurrentHealth -= damage;
		if(CurrentHealth <= 0){
			return true;
		}
		else{
			return false;
		}
	}
	
}
