using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public string characterName;
    Character _myCharacter;

    Skill _activeSkill;

        bool selectingFacing = false;

    private void Awake()
    {
        _myCharacter = new Character(characterName);
        TurnController.AddCharacter(this);
        MyCharacter.Attributes.StartBattle();
                transform.eulerAngles = new Vector3(0, MyCharacter.Facing, 0);
        
    }

    private void Update ()
    {
        if (selectingFacing) {
            if (Input.GetKeyDown (KeyCode.UpArrow)) {
                                transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y + 45, 0);
            }
                        if (Input.GetKeyDown (KeyCode.RightArrow)) {
                                transform.eulerAngles = new Vector3 (0, Camera.main.transform.eulerAngles.y + 135, 0);
                        }

                        if (Input.GetKeyDown (KeyCode.DownArrow)) {
                                transform.eulerAngles = new Vector3 (0, Camera.main.transform.eulerAngles.y + 215, 0);
                        }

                        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
                                transform.eulerAngles = new Vector3 (0, Camera.main.transform.eulerAngles.y + 305, 0);
                        }
            if (Input.GetKeyDown (KeyCode.Return)) {
                               selectingFacing = false;
                                TurnController.TurnOver();
            }
                }
        }

    public void StartMyTurn()
    {
        _myCharacter.Attributes.RefillAP();
    }

        public void StartSetFacingDirection ()
    {
                selectingFacing = true;
    }


        void FinaliseFacingDirection ()
        {
                if (transform.eulerAngles.y < 0) {
                        transform.eulerAngles += new Vector3 (0, 360, 0);
                }
                if(transform.eulerAngles.y >= 360) {
                        transform.eulerAngles -= new Vector3(0,360,0);
        }
                MyCharacter.SetFacing (Mathf.FloorToInt (transform.eulerAngles.y / 90));
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

        void SkillTargeted (Vector4 point)
        {
                if (_activeSkill.ProcessSkillEffect (this, point)) //skill succeeded
                {
                        _activeSkill.OnSkillTargeted -= SkillTargeted;
                        _activeSkill = null;
                } else { //skill failed. Go back to targeting
                        _activeSkill.StartSkillTargeting (this);
                }

        }

    public void SetAction(string actionName)
    {
        CancelSkill();
        if (_activeSkill == null && !selectingFacing)
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
