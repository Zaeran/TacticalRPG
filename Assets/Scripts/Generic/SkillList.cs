using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillList {
	Dictionary<string, string[]> skillInfo; //AP Cost. Is targeted (1,0). SkillType. SkillRange. SkillDifficulty
	//using skills variables
	string skillName;
	int skillCost;
	Vector3 target;
	GameObject origin;
	string weaponType;
	int baseDamage;

	int groundOnlyLayer = 1 << 8;

	TextAsset SkillData;

	// Use this for initialization
	void Awake () {
		skillInfo = new Dictionary<string, string[]>();

		SkillData = Resources.Load("Data/AllSkills") as TextAsset;
		LoadSkillData();
	}

	void LoadSkillData(){
		string[] lineData = SkillData.text.Split('\n');
		foreach(string skillData in lineData){
			string[] skill = skillData.Split(' ');
			if(skill.Length > 2){
				string[] data = new string[skill.Length - 1];
				for(int i = 0; i < data.Length; i++){
					data[i] = skill[i+1];
				}
				skillInfo.Add(skill[0], data);
			}
		}
	}

	public int getSkillCost(string skillName){
		return int.Parse(skillInfo[skillName][0]);
	}
	public string GetSkillRange(string skillName){
		return skillInfo[skillName][3];
	}
	public string GetSkillDifficulty(string skillName){
		return skillInfo[skillName][4];
	}

	public bool getIsSkillTargeted(string skillName){
		if(skillInfo[skillName][1] == "0"){
			return false;
		}
		return true;
	}

	//used to initialise a skill
	//required info: name of skill, target of skill, character skill originating from, and name of weapon (replace with equipment list later)
	public void PerformSkill(string name, Vector3 targt, GameObject orgn, int cost){
		skillName = name;
		target = targt;
		origin = orgn;
		skillCost = cost;
	}

	void SkillComplete(){
		skillName = "";
		skillCost = 0;
		target = new Vector3();
		origin = null;
		weaponType = "";
		baseDamage = 0;
	}

	#region regular skills
	IEnumerator Move(){
		origin.SendMessage("MoveAction");
		SkillComplete();
		yield return null;
	}
	IEnumerator Attack(){
		if(weaponType == "Melee"){
			Collider[] col;
			GameObject character = null;
			//test that the square contains a character
			col = Physics.OverlapSphere(target, 0.3f, ~groundOnlyLayer);
			foreach(Collider c in col){
				if(c.tag == "NPC" || c.tag == "Player"){
					character = c.gameObject;
					break;
				}
			}
			//allow character to react
			if(character != null){
				character.SendMessage("Reaction", character);
			}
			yield return new WaitForSeconds(1.5f); //ADD: animation
			//test that character is still in square
			col = Physics.OverlapSphere(target, 0.3f, ~groundOnlyLayer);
			character = null;
			foreach(Collider c in col){
				if(c.tag == "NPC" || c.tag == "Player"){
					character = c.gameObject;
					break;
				}
			}
			if(character != null){
				character.SendMessage("TakeDamage", baseDamage); //ADD: Damage modifiers
			}
			origin.SendMessage("ActionComplete", skillCost); //ADD: replace with skill cost
			GameObject.FindGameObjectWithTag("GUIData").SendMessage("UpdateTurn", origin);
		}
		else{
			const float projectileHeight = 0.4f;
			yield return new WaitForSeconds(1); //ADD: animation
			//create 'arrow', and fire it at the selected square
			//GameObject proj = Instantiate(Resources.Load("Objects/Weapons/Arrow"), origin.transform.position + new Vector3(0,projectileHeight,0), Quaternion.identity) as GameObject;
			//ProjectileScript Projectile = proj.GetComponent<ProjectileScript>();
			//if(origin.transform.position.y > target.y){
			//	Projectile.Initialise(60, target, projectileHeight, baseDamage, origin, skillCost);//ADD: Damage modifiers
			//}
			//else{
			//	Projectile.Initialise(75, target, projectileHeight, baseDamage, origin, skillCost);//ADD: Damage modifiers
			//}
		}
		SkillComplete();
		yield return null;
	}
	#endregion

	#region reaction skills
	
	#endregion
}
