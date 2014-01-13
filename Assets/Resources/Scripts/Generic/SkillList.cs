using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillList : MonoBehaviour {
	Dictionary<string, string[]> skillInfo; //AP Cost. Is targeted (1,0). SkillType. SkillRange. SkillDifficulty

	int groundOnlyLayer = 1 << 8;

	AllItemsList Items;
	CharacterEquipment Equipment;

	TextAsset SkillData;

	// Use this for initialization
	void Awake () {
		skillInfo = new Dictionary<string, string[]>();
		Items = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<AllItemsList>();

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
	public string getSkillRange(string skillName){
		return skillInfo[skillName][3];
	}

	public bool getIsSkillTargeted(string skillName){
		if(skillInfo[skillName][1] == "0"){
			return false;
		}
		return true;
	}

	//used to initialise a skill
	//required info: name of skill, target of skill, character skill originating from, and name of weapon (replace with equipment list later)
	public void PerformSkill(string skillName, Vector3 target, GameObject origin, int skillCost){
		Equipment = origin.GetComponent<CharacterEquipment>();
		int wpnDamage1 = 0;
		string wpnType1 = "";
		int wpnDamage2 = 0;
		string wpnType2 = "";
		string[] allEquipment = Equipment.GetAllEquipment();
		if(Items.GetItemType(allEquipment[0]) == "Weapon"){
			wpnType1 = Items.GetWpnType(allEquipment[0]);
			wpnDamage1 = Items.GetWpnDamage(allEquipment[0]);
		}
		if(Items.GetItemType(allEquipment[1]) == "Weapon"){
			wpnType2 = Items.GetWpnType(allEquipment[1]);
			wpnDamage2 = Items.GetWpnDamage(allEquipment[1]);
		}
		switch(skillName){ //ADD: Proper skill use when different weapon types in each hand
		case "Attack":
			if(wpnType1 == "Melee"){
				StartCoroutine(MeleeAttack(target, wpnDamage1, origin, skillCost));
			}
			else if(wpnType1 == "Ranged"){
				StartCoroutine(RangedAttack(target, wpnDamage1, origin, skillCost));
			}
			break;
		case "Block":
			StartCoroutine(Block(2, origin, skillCost));
			break;
		default: break;
		}
	}

	#region regular skills
	IEnumerator MeleeAttack(Vector3 target, int damage, GameObject origin, int skillCost){
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
		yield return new WaitForSeconds(1.5f); //animation
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
			character.SendMessage("TakeDamage", damage);
		}
		origin.SendMessage("ActionComplete", skillCost); //ADD: replace with skill cost
		GameObject.FindGameObjectWithTag("GUIData").SendMessage("UpdateTurn", origin);
	}

	IEnumerator RangedAttack(Vector3 target, int damage, GameObject origin, int skillCost){
		const float projectileHeight = 0.4f;
		yield return new WaitForSeconds(1); //replace with animation
		//create 'arrow', and fire it at the selected square
		GameObject proj = Instantiate(Resources.Load("Objects/Weapons/Arrow"), origin.transform.position + new Vector3(0,projectileHeight,0), Quaternion.identity) as GameObject;
		ProjectileScript Projectile = proj.GetComponent<ProjectileScript>();
		if(origin.transform.position.y > target.y){
		Projectile.Initialise(60, target, projectileHeight, damage, origin, skillCost);
		}
		else{
			Projectile.Initialise(75, target, projectileHeight, damage, origin, skillCost);
		}
	}
	#endregion

	#region reaction skills
	IEnumerator Block(int blockValue, GameObject origin, int skillCost){
		yield return new WaitForSeconds(0.2f); //block animation
		ReactionDamageReduction block = origin.AddComponent<ReactionDamageReduction>();
		block.Initialize(blockValue);
		origin.SendMessage("ActionComplete", skillCost);
	}
	#endregion
}
