using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
        [System.Flags] public enum SkillTag { Movement, Attack, Physical, Magical, Fire, Water, Earth, Air };
        string _name;
        SkillTag _tags;

        ISkillTargeting _targeting;

    public Skill (string sName, SkillTag tags, ISkillTargeting targeting)
    {
                _name = sName;
                _tags = tags;
                _targeting = targeting;
    }

}
