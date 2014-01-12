using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillList : MonoBehaviour {
	Dictionary<string, int> skills; //skill + AP cost
	Dictionary<string, bool> isSkillTargeted;


	int groundOnlyLayer = 1 << 8;

	AllItemsList Items;

	// Use this for initialization
	void Start () {
		skills = new Dictionary<string, int>();
		isSkillTargeted = new Dictionary<string, bool>();
		Items = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<AllItemsList>();
		skills.Add("Attack", 3);
		isSkillTargeted.Add("Attack", true);
		skills.Add("Block", 3);
		isSkillTargeted.Add("Block", false);
		skills.Add("Move", 1);
	}

	public int getSkillCost(string skillName){
		return skills[skillName];
	}

	public bool getIsSkillTargeted(string skillName){
		return isSkillTargeted[skillName];
	}

	//used to initialise a skill
	//required info: name of skill, target of skill, character skill originating from, and name of weapon (replace with equipment list later)
	public void PerformSkill(string skillName, Vector3 target, GameObject origin, int skillCost, string weaponName = ""){
		int wpnDamage = int.Parse(Items.GetWpnData(weaponName)[2]);
		int wpnType = int.Parse(Items.GetWpnData(weaponName)[1]);

		switch(skillName){
		case "Attack":
			if(wpnType == 1 || wpnType == 2){
				StartCoroutine(MeleeAttack(target, wpnDamage, origin, skillCost));
			}
			else if(wpnType == 3 || wpnType == 4){
				StartCoroutine(RangedAttack(target, wpnDamage, origin, skillCost));
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
			character.SendMessage("Reaction", origin);
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
		origin.SendMessage("ActionComplete", skillCost); //replace with skill cost
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
