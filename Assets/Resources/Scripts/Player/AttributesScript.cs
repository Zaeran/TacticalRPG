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

	FloatingNumbersScript floatingDisplay;
	
	private void Start(){
		CurrentHealth = MaxHealth;
		statusEffects = GetComponent<CharacterBuffsDebuffs>();
		floatingDisplay = GameObject.FindGameObjectWithTag("GameData").GetComponent<FloatingNumbersScript>();
	}
	
	public bool Damage(int damage){
		int damageTaken = damage - statusEffects.damageReductionPhysical;
		if(damageTaken < 0){
			damageTaken = 0;
		}
		CurrentHealth -= damageTaken;
		floatingDisplay.SetText(damageTaken.ToString(), transform.position + Vector3.up);
		if(CurrentHealth <= 0){
			return true;
		}
		else{
			return false;
		}
	}
	
}
