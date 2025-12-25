using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    [System.Flags] public enum SkillTag { Movement, Attack, Physical, Magical, Buff, Debuff, Fire, Water, Earth, Air };
    string _name;
    SkillTag _tags;

    int _mastery;

    List<ISkillPrerequisite> _prereqs;
    ISkillTargeting _targeting;
    ISkillTargetRadius _targetRadius;
    List<ISkillCost> _skillCost;
    List<ISkillEffect> _effects;

    public event Vector4Event OnSkillTargeted;

    public Skill(string sName)
    {
        _name = sName;
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

    public void SetTags(SkillTag tagFlag)
    {
        _tags = tagFlag;
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
                return false;
            }
        }
        return true;
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

}
