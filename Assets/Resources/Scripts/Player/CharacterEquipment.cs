using UnityEngine;
using System.Collections;

public class CharacterEquipment : MonoBehaviour {
	//slots: torso, head, feet, gloves, LHand, RHand
	string[] equipment = new string[6];

	//components
	AllItemsList Items;
	Inventory Inv;

	void Awake(){
		Items = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<AllItemsList>();
		Inv = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<Inventory>();
	}

	//get item in slot
	public string GetTorsoSlot(){
		return equipment[0];
	}
	public string GetHeadSlot(){
		return equipment[1];
	}
	public string GetHandSlot(){
		return equipment[2];
	}
	public string GetFootSlot(){
		return equipment[3];
	}
	public string GetLHandSlot(){
		return equipment[4];
	}
	public string GetRHandSlot(){
		return equipment[5];
	}
	//set new slot item
	public void SetTorsoSlot(string item){
		equipment[0] = item;
	}
	public void SetHeadSlot(string item){
		equipment[1] = item;
	}
	public void SetHandSlot(string item){
		equipment[2] = item;
	}
	public void SetFootSlot(string item){
		equipment[3] = item;
	}
	public void SetLHandSlot(string item){
		equipment[4] = item;
	}
	public void SetRHandSlot(string item){
		equipment[5] = item;
	}


}
