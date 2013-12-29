using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CharacterKnownAbilities : MonoBehaviour {

	//skills are associated a percentage value representing how well learned the skill is.
	//each successful use of this skill raises its percentage.
	//when swapping to a different weapon, the skill remains, but it's percentage is the chance of the skill succeeding.
	Dictionary<string, int> skillsKnown;
	Dictionary<string, int> magicKnown;

	SkillList skills;
	MagicScript magic;

	// Use this for initialization
	void Start () {
		skills = GameObject.FindGameObjectWithTag("GameData").GetComponent<SkillList>();
		magic = GameObject.FindGameObjectWithTag("GameData").GetComponent<MagicScript>();
		skillsKnown = new Dictionary<string, int>();
		magicKnown = new Dictionary<string, int>();
	}


}
