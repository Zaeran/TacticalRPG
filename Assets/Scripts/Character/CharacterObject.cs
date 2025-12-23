using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public string characterName;
    Character _myCharacter;

    Skill _activeSkill;

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
        if (_myCharacter.Attributes.CurrentAP == 0 && GameOptions.autoEndTurn)
        {
            TurnController.TurnOver();
        }
    }

    public Character MyCharacter
    {
        get { return _myCharacter; }
    }

    public void CancelSkill()
    {
        if (_activeSkill != null)
        {
            _activeSkill.OnSkillTargeted -= SkillTargeted;
            _activeSkill = null;
        }
        DrawSquaresScript.DestroyValidSquares();
    }

    void SkillTargeted(Vector4 point)
    {
        _activeSkill.OnSkillTargeted -= SkillTargeted;
        _activeSkill.ProcessSkillEffect(this, point);
        _activeSkill = null;
    }

    //Actions -- change this to be a separate script
    public void MoveAction(int distance = 0)
    {
        if (_activeSkill == null)
        {
            _activeSkill = MyCharacter.GetSkillByName("Move");
            _activeSkill.StartSkillTargeting(this);
            _activeSkill.OnSkillTargeted += SkillTargeted;
        }
    }
}
