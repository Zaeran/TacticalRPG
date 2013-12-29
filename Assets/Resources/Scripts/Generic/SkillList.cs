using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillList : MonoBehaviour {
	Dictionary<string, int> skills; //skill + AP cost


	int groundOnlyLayer = 1 << 8;

	WeaponList Weapons;
	MovementScript Move;

	// Use this for initialization
	void Start () {
		skills = new Dictionary<string, int>();
		Move = GetComponent<MovementScript>();
		Weapons = GameObject.FindGameObjectWithTag("Controller").GetComponent<WeaponList>();
	}

	//used to initialise a skill
	//required info: name of skill, target of skill, character skill originating from, and name of weapon (replace with equipment list later)
	public void PerformSkill(string skillName, Vector3 target, GameObject origin, string weaponName = ""){
		int wpnDamage = Weapons.GetWeaponCombatStats(weaponName)[1];
		int wpnType = Weapons.GetWeaponCombatStats(weaponName)[0];
		Debug.Log(wpnType);

		switch(skillName){
		case "Attack":
			if(wpnType == 1 || wpnType == 2){
				StartCoroutine(MeleeAttack(target, wpnDamage, origin));
			}
			else if(wpnType == 3 || wpnType == 4){
				StartCoroutine(RangedAttack(target, wpnDamage, origin));
			}
			break;
		case "Move":
			MoveAction(target);
			break;
		default: break;
		}
	}

	//skills
	void MoveAction(Vector3 position){

	}
	
	IEnumerator MeleeAttack(Vector3 target, int damage, GameObject origin){
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
		if(character != null){
			character.SendMessage("Reaction", origin);
		}
		yield return new WaitForSeconds(1.5f); //animation
		origin.SendMessage("ActionComplete", 3); //replace with skill cost
	}

	IEnumerator RangedAttack(Vector3 target, int damage, GameObject origin){
		const float projectileHeight = 0.4f;
		yield return new WaitForSeconds(1); //replace with animation
		int wpnDamage = damage;
		//create 'arrow', and fire it at the selected square
		GameObject proj = Instantiate(Resources.Load("Objects/Arrow"), origin.transform.position + new Vector3(0,projectileHeight,0), Quaternion.identity) as GameObject;
		ProjectileScript Projectile = proj.GetComponent<ProjectileScript>();
		Projectile.Initialise(60, target, projectileHeight, wpnDamage);
		
		yield return new WaitForSeconds(1); //allow time for arrow to hit
		origin.SendMessage("ActionComplete", 3); //replace with skill cost

	}
}
