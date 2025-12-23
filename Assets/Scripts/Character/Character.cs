using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    string _characterName;
    bool _isNPC;
    Attributes _attributes;

    public Character(string cName)
    {
        _characterName = cName;
        _attributes = new Attributes();
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
}
