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
		statusEffects = GetComponent<CharacterBuffsDebuffs>();
	}
	
	public bool Damage(int damage){
		int damageTaken = damage - statusEffects.damageReductionPhysical;
		if(damageTaken < 0){
			damageTaken = 0;
		}
		CurrentHealth -= damageTaken;
		if(CurrentHealth <= 0){
			return true;
		}
		else{
			return false;
		}
	}
	
}
