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

	//negative status effects
	public bool blind = false;
	public bool confuse = false;
	public bool disable = false;
	public bool enchanted = false;
	public bool immobilize = false;
	public bool poison = false;
	public bool silence = false;
	public bool sleep = false;
	public bool slow = false;
	public bool stun = false;
	public bool weak = false;

	//positive status effects
	public bool Protection = false;
	public bool Ward = false;
	public bool Strong = false;
	public bool Fast = false;

}
