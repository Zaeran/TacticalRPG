﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponList : MonoBehaviour {

	Dictionary<string, int> wpnType; //1 = 1H melee, 2 = 2H melee, 3 = 1H ranged, 4 = 2H ranged
	Dictionary<string, int> wpnDamage;
	Dictionary<string, int> wpnRange;
	Dictionary<string, int> wpnCost;
	Dictionary<string, int> wpnMagDamageType;
	Dictionary<string, int> wpnMagDamage;
	Dictionary<string, string> wpnSkill;

	TextAsset weaponData;


	// Use this for initialization
	void Awake () {
		wpnType = new Dictionary<string, int>();
		wpnDamage = new Dictionary<string, int>();
		wpnRange = new Dictionary<string, int>();
		wpnCost = new Dictionary<string, int>();
		wpnSkill = new Dictionary<string, string>();
		wpnMagDamage = new Dictionary<string, int>();
		wpnMagDamageType = new Dictionary<string, int>();
		weaponData = Resources.Load("Data/Weapons") as TextAsset;	
		LoadWeaponData();
		Debug.Log(wpnType["Bow"] + ", " + wpnDamage["Bow"]);
	}
	
	void LoadWeaponData(){
		//get each line individually
		string[] testString = weaponData.text.Split('\n');

		//get the data from the file
		foreach(string wpnData in testString){
			string[] wpn = wpnData.Split(' ');
			Debug.Log(wpn[0]);
			if(wpn.Length > 3){
				wpnType.Add(wpn[0], int.Parse(wpn[1]));
				wpnDamage.Add(wpn[0], int.Parse(wpn[2]));
				wpnRange.Add(wpn[0], int.Parse(wpn[3]));
				wpnMagDamageType.Add(wpn[0], int.Parse(wpn[4]));
				wpnMagDamage.Add(wpn[0], int.Parse(wpn[5]));
				wpnCost.Add(wpn[0], int.Parse(wpn[6]));
				wpnSkill.Add(wpn[0], wpn[7]);
			}
		}
	}

	public int[] GetWeaponCombatStats(string weapon){
		int[] stats = new int[5];
		stats[0] = wpnType[weapon];
		stats[1] = wpnDamage[weapon];
		stats[2] = wpnRange[weapon];
		stats[3] = wpnMagDamageType[weapon];
		stats[4] = wpnMagDamage[weapon];
		return stats;
	}

	public string GetWeaponSkill(string weapon){
		return wpnSkill[weapon];
	}
}
