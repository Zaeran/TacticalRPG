using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    [System.Flags] public enum SkillTag { Movement, Attack, Physical, Magical, Buff, Debuff, Fire, Water, Earth, Air };
    string _name;
    SkillTag _tags;

    int _apCost;
    ISkillTargeting _targeting;
    ISkillEffect _effect;

    public event Vector4Event OnSkillTargeted;

    public Skill(string sName, SkillTag tags, ISkillTargeting targeting, ISkillEffect effect)
    {
        _name = sName;
        _tags = tags;
        _targeting = targeting;
        _effect = effect;
    }

    public string Name
    {
        get { return _name; }
    }

    public int APCost
    {
        get { return _apCost; }
    }

    public void StartSkillTargeting(CharacterObject c)
    {
        _targeting.SelectTarget(c);
        MouseControlScript.OnTileClicked += TargetSelected;
    }

    public void TargetSelected(Vector4 position)
    {
        MouseControlScript.OnTileClicked -= TargetSelected;
        if(OnSkillTargeted != null)
        {
            OnSkillTargeted(position);
        }
    }

    public void ProcessSkillEffect(CharacterObject c, Vector4 point)
    {
        _effect.ProcessEffect(c, point);
    }

}
