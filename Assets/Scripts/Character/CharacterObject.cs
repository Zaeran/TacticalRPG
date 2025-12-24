using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public string characterName;
    Character _myCharacter;

    Skill _activeSkill;


    private void Awake()
    {
        _myCharacter = new Character(characterName);
        TurnController.AddCharacter(this);
        MyCharacter.Attributes.StartBattle();
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
        if(_activeSkill.ProcessSkillEffect(this, point)) //skill succeeded
        {
            _activeSkill.OnSkillTargeted -= SkillTargeted;
            _activeSkill = null;
        }
        else
        { //skill failed. Go back to targeting
            _activeSkill.StartSkillTargeting(this);
        }
        
    }

    public void SetAction(string actionName)
    {
        CancelSkill();
        if (_activeSkill == null)
        {
                        Skill s = MyCharacter.GetSkillByName(actionName);
            if (!s.TestPrerequisites (this)) {
                                Debug.Log("Prerequisites not met");
                                return;
            }
            _activeSkill = MyCharacter.GetSkillByName(actionName);
            _activeSkill.StartSkillTargeting(this);
            _activeSkill.OnSkillTargeted += SkillTargeted;
        }
    }
}
