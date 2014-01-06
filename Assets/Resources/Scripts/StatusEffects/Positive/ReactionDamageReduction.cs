using UnityEngine;
using System.Collections;

public class ReactionDamageReduction : MonoBehaviour {

	CharacterStatus Status;
	int reduction;

	public void Initialize(int value){
		reduction = value;
		Status = GetComponent<CharacterStatus>();
		Status.damageReductionPhysical += reduction;
	}

	public void ReactionComplete(){
		Status.damageReductionPhysical -= reduction;
		Destroy(this);
	}
}
