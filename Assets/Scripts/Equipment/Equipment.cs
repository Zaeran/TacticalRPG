using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{ 
    protected string _name;
    protected EquipmentType _equipmentType;
    protected List<SkillTree> _allowedSkillTrees;

    public Equipment(string name, EquipmentType eType, List<SkillTree> allowedSkillTrees)
    {
        _name = name;
        _equipmentType = eType;
        _allowedSkillTrees = allowedSkillTrees;
    }

    public string Name
    {
        get { return _name; }
    }

    public EquipmentType EquipmentType
    {
        get { return _equipmentType; }
    }

    public List<SkillTree> AllowedSkillTrees
    {
        get { return _allowedSkillTrees; }
    }
}
