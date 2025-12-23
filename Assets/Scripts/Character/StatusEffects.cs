using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects
{
	//damage reductions
	int damageReductionPhysical = 0;
	int damageReductionMagical = 0;

	//action related, affects actions player can take.
	public bool actionOccuring = false;
	public bool isReacting = false;
	public bool isMyTurn = false;
	public bool targetReactionOccuring = false;

	//negative status effects
	public int blind = 0; //Attacks miss automatically.
	public int confuse = 0; //Let's do something a little unique with this
	public int disable = 0; //Can't use hands / attack. Can still move.
	public int enchanted = 0; //Controlled by opponent
	public int immobilize = 0; //Can't move
	public int poison = 0; //Damage each turn / damage per AP spent
	public int silence = 0; //Can't use spells
	public int sleep = 0; //Asleep - can be woken by attacking/moving
	public int slow = 0; //Double AP cost
	public int stun = 0; //Skip turn
	public int weak = 0; //Deal less physical damage
	public int fear = 0; //Build on this later. Fear of heights - reduce jump. Fear of danger - can't move towards an enemy. etc.

	//positive status effects
	public int protection = 0; //Take less physical damage
	public int ward = 0; //Take less magical damage
	public int strong = 0; //Deal more physical damage
	public int fast = 0; //Half AP cost
	public int heightened = 0; //Deal more magical damage
	public int blocking = 0; //Can't take damage from facing direction
	public int evading = 0; //Guaranteed to dodge next attack
}
