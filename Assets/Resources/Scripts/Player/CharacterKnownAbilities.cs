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
	MagicList magic;
	WeaponList Weapons;

	// Use this for initialization
	void Start () {
		skills = GameObject.FindGameObjectWithTag("Controller").GetComponent<SkillList>();
		magic = GameObject.FindGameObjectWithTag("Controller").GetComponent<MagicList>();
		Weapons = GameObject.FindGameObjectWithTag("Controller").GetComponent<WeaponList>();
		skillsKnown = new Dictionary<string, int>();
		magicKnown = new Dictionary<string, int>();
		skillsKnown.Add("Attack", 100);
	}

	//checks that the skill succeeds
	public bool SkillSucceeds(string skillName, string weaponName){
		int modifier = 0;
		string weaponSkill = Weapons.GetWeaponSkill(weaponName);
		if(weaponSkill == skillName){
			modifier += 40;
		}
		if(Random.Range(0,101) < skillsKnown[skillName] + modifier){
			return true;
		}
		return false;
	}

	public void IncreaseAbility(){

	}
}
