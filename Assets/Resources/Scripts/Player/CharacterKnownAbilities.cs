﻿using UnityEngine;
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
	AllItemsList Items;
	CharacterEquipment Equipment;

	// Use this for initialization
	void Start () {
		skills = GameObject.FindGameObjectWithTag("Controller").GetComponent<SkillList>();
		magic = GameObject.FindGameObjectWithTag("Controller").GetComponent<MagicList>();
		Items = GameObject.FindGameObjectWithTag("Controller").GetComponent<AllItemsList>();
		Equipment = GetComponent<CharacterEquipment>();
		skillsKnown = new Dictionary<string, int>();
		magicKnown = new Dictionary<string, int>();
		skillsKnown.Add("Attack", 100);
		skillsKnown.Add("Block", 100);
	}

	//checks that the skill succeeds
	public bool SkillSucceeds(string skillName){
		int modifier = 0;
		foreach(string skill in Equipment.GetAllEquipmentSkills()){
			if(skill == skillName){
				modifier += 40 / int.Parse(skills.GetSkillDifficulty(skillName));  //ADD: Different Skill Modifiers
			}
		}
		if(Random.Range(0,100) < skillsKnown[skillName] + modifier){
			return true;
		}
		return false;
	}

	public void IncreaseAbility(string skillName, int amount){
		skillsKnown[skillName] += amount;
		if(skillsKnown[skillName] > 100){
			skillsKnown[skillName] = 100;
		}
	}
}
