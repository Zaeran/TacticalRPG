using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllItemsList : MonoBehaviour {
	//item slots: 1- hands (wpn/shield), 2-torso, 3-head, 4-
	Dictionary<string, string> allItemCategories = new Dictionary<string, string>();
	Dictionary<string, string[]> weapons = new Dictionary<string, string[]>(); //name, type, hands, wpnType, damage, range, skill, cost, other effect
	Dictionary<string, string[]> armour = new Dictionary<string, string[]>(); //name, type, slot, physDef, magDef, skill, cost, other effect
	Dictionary<string, string[]> accessories = new Dictionary<string, string[]>();
	Dictionary<string, string[]> items = new Dictionary<string, string[]>();

	TextAsset ItemData;

	void Awake(){
		ItemData = Resources.Load("Data/AllItems") as TextAsset;
		LoadItemData();
	}
	void LoadItemData(){
		string[] lineData = ItemData.text.Split('\n');
		foreach(string itmData in lineData){
			string[] item = itmData.Split(' ');
			if(item.Length > 2){
				allItemCategories.Add(item[0], item[1]);
				switch(item[1]){
				case "Weapon":
					weapons.Add(item[0], item);
					break;
				case "Armour":
					armour.Add(item[0], item);
					break;
				case "Accessory":
					accessories.Add(item[0], item);
					break;
				case "Item":
					items.Add(item[0], item);
					break;
				default:
					break;
				}
			}
		}
	}

	public string[] GetWeaponList(){
		string[] list = new string[weapons.Keys.Count];
		weapons.Keys.CopyTo(list,0);
		return list;
	}
	public string[] GetItemData(string item){
		switch(allItemCategories[item]){
		case "Weapon":
			return weapons[item];
		case "Armour":
			return armour[item];
		case "Accessory":
			return accessories[item];
		case "Item":
			return items[item];
		default:
			break;
		}
		return new string[0];
	}
	public string GetItemType(string item){
		if(item != ""){
			return allItemCategories[item];
		}
		return "";
	}
	public string GetItemSkill(string item){
		switch(allItemCategories[item]){
		case "Weapon":
			return weapons[item][7];
		case "Armour":
			return armour[item][0];
		case "Accessory":
			return accessories[item][0];
		case "Item":
			return items[item][0];
		default:
			break;
		}
		return "";
	}
	public string GetItemCost(string item){
		return ""; //ADD: Item Cost
	}

	//get individual weapon stats

	public int GetWpnHands(string weapon){
		return int.Parse(weapons[weapon][2]);
	}
	public string GetWpnType(string weapon){
		return weapons[weapon][3];
	}
	public int GetWpnDamage(string weapon){
		return int.Parse(weapons[weapon][4]);
	}

	public int GetWpnRange(string weapon){
		return int.Parse(weapons[weapon][5]);
	}



}
