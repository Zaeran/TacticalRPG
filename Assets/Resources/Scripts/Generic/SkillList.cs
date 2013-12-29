using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillList : MonoBehaviour {
	Dictionary<string, int> skills; //skill + AP cost

	// Use this for initialization
	void Start () {
		skills = new Dictionary<string, int>();
	}

	//used to initialise a skill
	public void PerformSkill(GameObject target){

	}

	public void PerformSkill(Vector3 position){

	}

	//skills
	void MeleeAttack(GameObject target){

	}
}
