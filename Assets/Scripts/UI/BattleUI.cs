using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{

    public TextMeshProUGUI characterNameText;

    private void Start()
    {
        TurnController.OnNextCharacterTurn += SetCurrentTurnName;
    }

    public void PassTurn()
    {
        TurnController.TurnOver();
    }

    public void MoveAction()
    {
        TurnController.CurrentCharacterTurn.MoveAction();
    }

    void SetCurrentTurnName(Character c)
    {
        characterNameText.text = c.CharacterName;
    }
}
