using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    string _name;
    int _mastery;

    List<SkillTag> _tags;
    List<ISkillPrerequisite> _prereqs;
    ISkillTargeting _targeting;
    ISkillTargetRadius _targetRadius;
    List<ISkillCost> _skillCost;
    List<ISkillEffect> _effects;

    public event Vector4Event OnSkillTargeted;

    public Skill(string sName)
    {
        _name = sName;
        _tags = new List<SkillTag>();
        _prereqs = new List<ISkillPrerequisite>();
        _skillCost = new List<ISkillCost>();
        _effects = new List<ISkillEffect>();
    }

    public string Name
    {
        get { return _name; }
    }

    public void AddPrerequisite(ISkillPrerequisite prereq)
    {
        _prereqs.Add(prereq);
    }

    public void AddSkillTags(SkillTag skillTag)
    {
        _tags.Add(skillTag);
    }

    public void SetTargeting(ISkillTargeting targeting)
    {
        _targeting = targeting;
    }

    public void AddSkillEffect(ISkillEffect effect)
    {
        _effects.Add(effect);
    }

    public bool TestPrerequisites(CharacterObject c)
    {
        for (int i = 0; i < _prereqs.Count; i++)
        {
            if (!_prereqs[i].MeetsPrerequisite(c))
            {
                StatusText.SetStatusText("Prerequisite not met: " + _prereqs[i].GetPrerequisiteFailureText(), 1.5f);
                return false;
            }
        }
        return true;
    }

    public bool CanUseWithSkillTrees(List<SkillTree> t)
    {
        int totalPrereqCount = 0;
        int validPrereqCount = 0;
        for(int i = 0; i < _prereqs.Count; i++)
        {
            if(_prereqs[i] is SkillPrereqActiveSkillTree)
            {
                totalPrereqCount++;
                foreach (SkillTree tree in t)
                {
                    if (((SkillPrereqActiveSkillTree)_prereqs[i]).MeetsPrerequisite(tree))
                    {
                        validPrereqCount++;
                    }
                }
            }
        }

        return totalPrereqCount == validPrereqCount;
    }

    public void AddSkillCost(ISkillCost cost)
    {
        _skillCost.Add(cost);
    }

    public void SetSkillTargetRadius(ISkillTargetRadius targetRadius)
    {
        _targetRadius = targetRadius;
    }

    public void StartSkillTargeting(CharacterObject c)
    {
        _targeting.SelectTarget(c);
        MouseControlScript.OnTileClicked += TargetSelected;
    }

    public void TargetSelected(Vector4 position)
    {
        MouseControlScript.OnTileClicked -= TargetSelected;
        if (OnSkillTargeted != null)
        {
            OnSkillTargeted(position);
        }
    }

    public void CancelSkill()
    {
        MouseControlScript.OnTileClicked -= TargetSelected;
    }

    public bool RequiresTargetConfirmation
    {
        get{return _targetRadius.RequiresConfirmation();}
    }

    public List<Vector4> ValidSquaresForPosition(CharacterObject c, Vector4 point)
    {
        return _targetRadius.ValidSquares(c, point);
    }

    public bool PrepareConfirmingTargets(CharacterObject c, Vector4 point)
    {
        List<ClickableTarget> hitCharacters = _targetRadius.GetTargets(c, point);
        if (hitCharacters == null) //No valid character found
        {
            Debug.Log("no valid targets");
            StatusText.SetStatusText("No valid targets", 1.0f);
            return false;
        }
        TargetIndicatorManager.instance.CreateTargetIndicators(hitCharacters);
        return true;
    }

    public bool ProcessSkillEffect(CharacterObject c, Vector4 point)
    {
        bool canPayCost = true;
        for (int i = 0; i < _skillCost.Count; i++)
        {
            if (!_skillCost[i].CanPayCost(c, point)) //If any costs can't be paid, skill can't activate
            {
                canPayCost = false;
                break;
            }
        }

        if (!canPayCost) //Can't pay the cost. Skill can't activate
        {
            return false;
        }

        //Figure out valid targets before costs are paid
        List<ClickableTarget> hitCharacters = _targetRadius.GetTargets(c, point);
        if (hitCharacters == null) //No valid character found
        {
            Debug.Log("no valid targets");
            StatusText.SetStatusText("No valid targets", 1.0f);
            return false;
        }

        for (int i = 0; i < _skillCost.Count; i++) //Pay the associated costs
        {
            if(!_skillCost[i].PayCost(c, point)) //If can't actually pay the cost (due to interrupt or other), skill fails
            {
                return false;
            }
        }

        //Do the thing
        for (int i = 0; i < _effects.Count; i++)
        {
            _effects[i].ProcessEffect(c, hitCharacters, point);
        }
        return true;
    }

    public string SkillDescription()
    {
        CharacterObject currentCharacter = TurnController.CurrentCharacterTurn;
        string sName = _name;
        string prereqs = "";
        for(int i = 0; i < _prereqs.Count; i++)
        {
            prereqs += "- " + _prereqs[i].Description(currentCharacter) + "\n";
        }
        string costs = "";
        for(int i = 0; i < _skillCost.Count; i++)
        {
            costs += "- " + _skillCost[i].Description(currentCharacter) + "\n";
        }
        string targeting = _targeting.Description(currentCharacter);
        string targetRadius = _targetRadius.Description(currentCharacter);
        string effects = "";
        for(int i = 0; i < _effects.Count; i++)
        {
            effects += "- " + _effects[i].Description(currentCharacter) + "\n";
        }
        return string.Format("{0}\nPREREQUISITES\n{1}COSTS\n{2}\nRANGE: {3}\nRADIUS: {4}\nEFFECTS\n{5}", sName, prereqs, costs, targeting, targetRadius, effects);
    }

}
