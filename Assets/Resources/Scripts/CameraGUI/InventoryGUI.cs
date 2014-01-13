using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryGUI : MonoBehaviour {

	bool inventoryOpen = false;
	string[] categoryList = new string[]{"Weapons", "Armour", "Accessories", "Items"};
	string currentCategory = "Weapons";
	Inventory Inv;
	AllItemsList ItemsList;

	//DESIGN: Inventory GUI
	public void ToggleInventory(){
		inventoryOpen = !inventoryOpen;
		Inv = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<Inventory>();
		ItemsList = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<AllItemsList>();
	}

	void OnGUI(){
		//only show if inventory is open
		if(inventoryOpen){
			//show all items
			Dictionary<string, int> Items = Inv.GetItemsByCategory(currentCategory);
			List<Rect> itemIcons = new List<Rect>();
		}
	}
}
