using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{ 
    protected string _name;
    protected EquipmentType _equipmentType;

    public Equipment(string name, EquipmentType eType)
    {
        _name = name;
        _equipmentType = eType;
    }
}
