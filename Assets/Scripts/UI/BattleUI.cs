using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{

    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characteAPText;
        public TextMeshProUGUI characterHPText;
    public TextMeshProUGUI characterEquipmentText;
    public GameObject skillObject;

    private void Awake()
    {
        TurnController.OnCharacterTurnEnding += CharacterTurnEnding;
        TurnController.OnNextCharacterTurn += CharacterTurnStarted;
    }

    public void PassTurn()
    {
        //TurnController.TurnOver();
        TurnController.CurrentCharacterTurn.StartSetFacingDirection();
    }

    public void UseAction(string action)
    {
        TurnController.CurrentCharacterTurn.SetAction(action);
    }

    public void CancelAction()
    {
        TurnController.CurrentCharacterTurn.CancelSkill();
    }

    void CharacterTurnStarted(Character c)
    {
        characterNameText.text = c.CharacterName;
        CharacterAPChanged();
        CharacterHPCHanged();
        characterEquipmentText.text = "Weapon: " + c.Weapon.Name;
        c.Attributes.OnRemainingAPChanged += CharacterAPChanged;
        c.Attributes.OnRemainingHPChanged += CharacterHPCHanged;
        skillObject.SetActive(false);
    }

    void CharacterTurnEnding(Character c)
    {
        c.Attributes.OnRemainingAPChanged -= CharacterAPChanged;
                c.Attributes.OnRemainingHPChanged -= CharacterHPCHanged;
    }

    void CharacterAPChanged()
    {
        characteAPText.text = "AP: " + TurnController.CurrentCharacterTurn.MyCharacter.Attributes.CurrentAP + "/" + TurnController.CurrentCharacterTurn.MyCharacter.Attributes.MaxAP;
        skillObject.SetActive(false);
    }

    void CharacterHPCHanged()
    {
        characterHPText.text = "HP: " + TurnController.CurrentCharacterTurn.MyCharacter.Attributes.HealthCurrent + "/" + TurnController.CurrentCharacterTurn.MyCharacter.Attributes.HealthMax;
    }
}
