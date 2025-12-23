using UnityEngine;
using System.Collections;

public class AttributesScript : MonoBehaviour {

	//base stats
	public int MaxHealth;
	public int CurrentHealth;
	public int maxActions;
	public int remainingAP;
	public int speedStat;
	public int maxJump;

	//Components
	FloatingNumbersScript floatingDisplay;
	
	private void Start(){
		CurrentHealth = MaxHealth;
		floatingDisplay = GameObject.FindGameObjectWithTag("GUIData").GetComponent<FloatingNumbersScript>();
	}
	
	public bool Damage(int damage){
		int damageTaken = damage;
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
