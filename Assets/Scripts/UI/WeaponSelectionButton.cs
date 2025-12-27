using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSelectionButton : MonoBehaviour
{
    EquipmentWeapon _myWeapon;
    BattleUI _ui;
    public TextMeshProUGUI myText;

    public void Initialize(EquipmentWeapon weapon, BattleUI ui)
    {
       _myWeapon = weapon;
       _ui = ui; 
       myText.text = weapon.Name;
    }

    public void SelectWeapon()
    {
        TurnController.CurrentCharacterTurn.MyCharacter.EquipNewWeapon(_myWeapon);
    }

    public void ShowWeaponInfo()
    {
        _ui.SetSkillDescriptionText(_myWeapon.WeaponInformation);
    }

    public void HideWeaponInfo()
    {
        _ui.SetSkillDescriptionText("");
    }
}
