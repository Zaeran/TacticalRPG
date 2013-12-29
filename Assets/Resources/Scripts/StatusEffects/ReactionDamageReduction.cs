using UnityEngine;
using System.Collections;

public class ReactionDamageReduction : MonoBehaviour {

	CharacterBuffsDebuffs status;
	int reduction;

	public void Initialize(int value){
		reduction = value;
		status = GetComponent<CharacterBuffsDebuffs>();
		status.damageReductionPhysical += reduction;
	}

	public void ReactionComplete(){
		status.damageReductionPhysical -= reduction;
		Destroy(this);
	}
}
