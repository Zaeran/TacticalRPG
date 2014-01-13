using UnityEngine;
using System.Collections;

public class EquipmentGUI : MonoBehaviour {
	
	GameObject Character;
	CharacterEquipment Equipment;

	//DESIGN: Equipment GUI
	void SelectCharacter(GameObject character){
		Character = character;
		Equipment = Character.GetComponent<CharacterEquipment>();
	}

}
