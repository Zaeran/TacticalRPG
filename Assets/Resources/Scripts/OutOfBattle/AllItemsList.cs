using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllItemsList : MonoBehaviour {
	Dictionary<string, string[]> weapons = new Dictionary<string, string[]>(); //name, type (hands/melee/range), damage, range, skill, cost
	Dictionary<string, string[]> armour = new Dictionary<string, string[]>(); //name, slot, defense, skill, cost
	Dictionary<string, string[]> accessories = new Dictionary<string, string[]>();
	Dictionary<string, string[]> items = new Dictionary<string, string[]>();

	TextAsset weaponData;
	TextAsset armourData;
	TextAsset accessoryData;
	TextAsset itemData;

	void Awake(){
		weaponData = Resources.Load("Data/Weapons") as TextAsset;
		armourData = Resources.Load("Data/Armour") as TextAsset;
		accessoryData = Resources.Load("Data/Accessories") as TextAsset;
		itemData = Resources.Load("Data/Items") as TextAsset;
		LoadWeaponData();
		LoadArmourData();
		LoadAccessoryData();
		LoadItemData();
	}
	void LoadWeaponData(){
		string[] lineData = weaponData.text.Split('\n');
		foreach(string wpnData in lineData){
			string[] wpn = wpnData.Split(' ');
			if(wpn.Length > 3){
				weapons.Add(wpn[0], wpn);
			}
		}
	}
	void LoadArmourData(){
		string[] lineData = armourData.text.Split('\n');
		foreach(string armData in lineData){
			string[] arm = armData.Split(' ');
			if(arm.Length > 3){
				armour.Add(arm[0], arm);
			}
		}
	}
	void LoadAccessoryData(){
		string[] lineData = accessoryData.text.Split('\n');
		foreach(string accData in lineData){
			string[] accessory = accData.Split(' ');
			if(accessory.Length > 3){
				accessories.Add(accessory[0], accessory);
			}
		}
	}
	void LoadItemData(){
		string[] lineData = itemData.text.Split('\n');
		foreach(string itmData in lineData){
			string[] item = itmData.Split(' ');
			if(item.Length > 3){
				items.Add(item[0], item);
			}
		}
	}

	public string[] GetWeaponList(){
		string[] list = new string[weapons.Keys.Count];
		weapons.Keys.CopyTo(list,0);
		return list;
	}
	public string[] GetWpnData(string item){
		return weapons[item];
	}
	public string[] GetArmourData(string item){
		return armour[item];
	}
	public string[] GetAccessoryData(string item){
		return accessories[item];
	}
	public string[] GetItemData(string item){
		return items[item];
	}
}
