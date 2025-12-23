using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    string _characterName;
    bool _isNPC;
    Attributes _attributes;
    List<Skill> _skills;

    public Character(string cName)
    {
        _characterName = cName;
        _attributes = new Attributes();
        _skills = new List<Skill>();
        SetupSkills();
    }

    void SetupSkills()
    {
        Skill moveSkill = new Skill("Move", Skill.SkillTag.Movement, new SkillTargetMoveRange(), new SkillEffectMove());
        _skills.Add(moveSkill);
    }

    public string CharacterName
    {
        get { return _characterName; }
    }

    public bool IsNPC
    {
        get { return _isNPC; }
    }

    public bool IsPlayable
    {
        get { return !_isNPC; }
    }

    public Attributes Attributes
    {
        get { return _attributes; }
    }

    public List<Skill> Skills
    {
        get
        {
            return _skills;
        }
    }

    public Skill GetSkillByName(string name)
    {
        for(int i = 0; i < _skills.Count; i++)
        {
            if(_skills[i].Name == name)
            {
                return _skills[i];
            }
        }
        return null;
    }
}
