using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {
	//damage reductions
	public int damageReductionPhysical = 0;
	public int damageReductionMagical = 0;

	//action related, affects actions player can take.
	public bool actionOccuring = false;
	public bool isReacting = false;
	public bool isMyTurn = false;
	public bool targetReactionOccuring = false;

}
