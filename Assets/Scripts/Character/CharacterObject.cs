using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : ClickableTarget
{
    public string characterName;
    Character _myCharacter;

    Skill _activeSkill;

    bool selectingFacing = false;
    bool confirmingTarget;

    private void Awake()
    {
        _myCharacter = new Character(characterName);
        TurnController.AddCharacter(this);
        MyCharacter.Attributes.StartBattle();
        transform.eulerAngles = new Vector3(0, MyCharacter.Facing, 0);

    }

    private void Update()
    {
        if (selectingFacing)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45, 0);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 135, 0);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 215, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 305, 0);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                selectingFacing = false;
                TurnController.TurnOver();
            }
        }
        if (confirmingTarget)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Confirm");
                ConfirmTargets(proposedTargetPoint);
                confirmingTarget = false;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                 confirmingTarget = false;
                 CancelSkill();
            }
        }
    }

    public void StartMyTurn()
    {
        _myCharacter.Attributes.RefillAP();
    }

    public void StartSetFacingDirection()
    {
        selectingFacing = true;
        DrawSquaresScript.DestroyValidSquares();
        StatusText.SetStatusText("Set facing direction");
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
            StatusText.SetStatusText("");
        }
        DrawSquaresScript.DestroyValidSquares();
        TargetIndicatorManager.RemoveIndicators();
    }

    void SkillTargeted(Vector4 point)
    {
        if(_activeSkill.RequiresTargetConfirmation)
        {
            proposedTargetPoint = point;
            if(_activeSkill.PrepareConfirmingTargets(this, point))
            {
                confirmingTarget = true;
            }
            StatusText.SetStatusText("Confirm targets? (Press ENTER or ESC)");
        }
        else
        {
            ConfirmTargets(point);
        }
    }

Vector4 proposedTargetPoint;

    void ConfirmTargets(Vector4 point)
    {
         //show valid targets, then confirm
        if (_activeSkill.ProcessSkillEffect(this, point)) //skill succeeded
        {
            _activeSkill.OnSkillTargeted -= SkillTargeted;
            _activeSkill = null;
            StatusText.SetStatusText("");
        }
        else
        { //skill failed. Go back to targeting
            _activeSkill.StartSkillTargeting(this);
            StatusText.SetStatusText("Action: " + _activeSkill.Name);
        }
        TargetIndicatorManager.RemoveIndicators();
    }

    public void SetAction(string actionName)
    {
        CancelSkill();
        if (_activeSkill == null && !selectingFacing)
        {
            Skill s = MyCharacter.GetSkillByName(actionName);
            if (!s.TestPrerequisites(this))
            {
                return;
            }
            _activeSkill = MyCharacter.GetSkillByName(actionName);
            _activeSkill.StartSkillTargeting(this);
            _activeSkill.OnSkillTargeted += SkillTargeted;

            StatusText.SetStatusText("Action: " + _activeSkill.Name);
        }
    }

    public void Fall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
        {
            transform.position = hit.point;
            //Deal fall damage
            if (hit.distance > MyCharacter.JumpStat)
            {
                int fallDamage = Mathf.FloorToInt(hit.distance - MyCharacter.JumpStat);
                MyCharacter.AdjustHitPoints(-fallDamage);
                FloatingNumbersScript.CreateFloatingNumber(transform.position, fallDamage.ToString(), Color.red);
            }
        }
    }
}
