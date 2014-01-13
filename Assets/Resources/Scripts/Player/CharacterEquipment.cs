using UnityEngine;
using System.Collections;

public class CharacterEquipment : MonoBehaviour {
	//slots: RHand, LHand, Armour, 3x Accessory
	public string[] equipment = new string[]{"","","","","",""};

	//components
	AllItemsList Items;
	Inventory Inv;

	void Awake(){
		Items = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<AllItemsList>();
		Inv = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<Inventory>();
	}

	public string[] GetAllEquipment(){
		return equipment;
	}

	public string[] GetAllEquipmentSkills(){
		string[] skills = new string[]{"none","none","none","none","none","none"};
		for(int i = 0; i < 6; i++){
			if(equipment[i] != ""){
				skills[i] = Items.GetItemSkill(equipment[i]);
			}
		}
		return skills;
	}

	public void EquipItem(string item, int slot){
		if(Inv.GetItemEquipped(item) < Inv.GetItemQuantity(item)){
		equipment[slot] = item;
		Inv.EquipItem(item);
		}
		else{
			Debug.Log("Not enough of ITEM");
		}
	}
	public void UnEquipItem(int slot){
		if(equipment[slot] != ""){
			Inv.UnEquipItem(equipment[slot]);
			equipment[slot] = "";
		}
	}
}
