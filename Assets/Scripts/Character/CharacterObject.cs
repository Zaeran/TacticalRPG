using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public string characterName;
    Character _myCharacter;

    MovementScript movement;


    private void Awake()
    {
        _myCharacter = new Character(characterName);
        TurnController.AddCharacter(this);

        movement = GetComponent<MovementScript>();
    }

    public void StartMyTurn()
    {
        _myCharacter.Attributes.RefillAP();
    }

    public void UseAP(int amount)
    {
        _myCharacter.Attributes.ModifyAP(amount);
        Debug.Log(_myCharacter.CharacterName + " remaining AP: " + _myCharacter.Attributes.CurrentAP);

        if (_myCharacter.Attributes.CurrentAP == 0 && GameOptions.autoEndTurn)
        {
            TurnController.TurnOver();
        }
    }

    public Character MyCharacter
    {
        get { return _myCharacter; }
    }

    //Actions -- change this to be a separate script
    public void MoveAction(int distance = 0)
    {
        //skillSelected = "Move";
        if (distance == 0)
        {
            distance = _myCharacter.Attributes.CurrentAP;
        }
        Debug.Log("Current AP: " + distance);
        List<Vector4> validPoints = FindValidPoints.GetPoints("Move", gameObject, distance, _myCharacter.Attributes.MaxJump);
        DrawSquaresScript.DrawValidSquares(validPoints);
      //  Mouse.SelectPosition(validPoints);
    }

}
