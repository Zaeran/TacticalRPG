using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{

    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characteAPText;

    private void Start()
    {
        TurnController.OnCharacterTurnEnding += CharacterTurnStarted;
        TurnController.OnNextCharacterTurn += CharacterTurnStarted;
    }

    public void PassTurn()
    {
        TurnController.TurnOver();
    }

    public void MoveAction()
    {
        TurnController.CurrentCharacterTurn.SetAction("Move");
    }

    public void AttackAction()
    {
        TurnController.CurrentCharacterTurn.SetAction("Attack");
    }

    public void CancelAction()
    {
        TurnController.CurrentCharacterTurn.CancelSkill();
    }

    void CharacterTurnStarted(Character c)
    {
        characterNameText.text = c.CharacterName;
        c.Attributes.OnRemainingAPChanged += CharacterAPChanged;
    }

    void CharacterTurnEnding(Character c)
    {
        c.Attributes.OnRemainingAPChanged -= CharacterAPChanged;
    }

    void CharacterAPChanged()
    {
        characteAPText.text = TurnController.CurrentCharacterTurn.MyCharacter.Attributes.CurrentAP + "/" + TurnController.CurrentCharacterTurn.MyCharacter.Attributes.MaxAP; 
    }
}
