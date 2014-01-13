using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	//dictionary - name and quantity
	List<string> items;
	Dictionary<string, int> itemQuantity;
	Dictionary<string, int> itemsEquipped;
	Dictionary<string, string> itemType;
	Dictionary<string, List<string>> categorisedItems;

	AllItemsList AllItems;

	// Use this for initialization
	void Start () {
		AllItems = GetComponent<AllItemsList>();

		items = new List<string>();
		itemQuantity = new Dictionary<string, int>();
		itemsEquipped = new Dictionary<string, int>();
		itemType = new Dictionary<string, string>();
		categorisedItems = new Dictionary<string, List<string>>();

		//set categories
		categorisedItems.Add("Armour", new List<string>());
		categorisedItems.Add("Weapon", new List<string>());
		categorisedItems.Add("Accessory", new List<string>());
		categorisedItems.Add("Item", new List<string>());
	}

	//add an item to the inventory
	public void AddItem(string name, int quantity = 1){
		//item exists
		if(items.Contains(name)){
			itemQuantity[name] += quantity;
		}
		//item doesn't exist
		else{
			items.Add(name);
			itemQuantity.Add(name, quantity);
			itemsEquipped.Add(name, 0);
			itemType.Add(name, GetItemType(name));
			categorisedItems[GetItemType(name)].Add(name);
		}
	}
	//return items and quantity of each item in a specific category
	public Dictionary<string, int> GetItemsByCategory(string category){
		Dictionary<string, int> items = new Dictionary<string, int>();
		foreach(string s in categorisedItems[category]){
			items.Add(s, itemQuantity[s]);
		}
		return items;
	}

	public int GetItemQuantity(string item){
		return itemQuantity[item];
	}
	public int GetItemEquipped(string item){
		return itemsEquipped[item];
	}
	public string GetItemType(string item){
		return AllItems.GetItemType(item);
	}

	public void EquipItem(string item){
		itemsEquipped[item] += 1;
	}

	public void UnEquipItem(string item){
		itemsEquipped[item] -= 1;
	}
}
