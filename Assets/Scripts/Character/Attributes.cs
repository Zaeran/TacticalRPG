using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes
{
    //Core attributes
    private int _hitPointsMax; //health
    private int _strength; //physical damage
    private int _reaction; //Skill recovery
    private int _dexterity; //jump + max AP
    private int _creativity; //magical damage
    private int _level;
    private int _xp;

    //secondary attributes - combat only
    private int _hitPointsCurrent;
    private int _actionPointsCurrent;

    public event VoidEvent OnRemainingAPChanged;

    public Attributes()
    {
        _hitPointsMax = 5;
        _strength = 5;
        _reaction = 5;
        _dexterity = 5;
        _creativity = 5;
        _level = 1;
        _xp = 0;
    }

    //TBD
    //defence
    //initiative
    //magical resistance

    //Derived attributes

    public int PhysicalDamage
    {
        get { return _strength; }
    }
    public int HealthMax
    {
        get
        {
            return _hitPointsMax;
        }
    }

    public int HealthCurrent
    {
        get
        {
            return _hitPointsCurrent;
        }
    }

    public void StartBattle()
    {
        _hitPointsCurrent = _hitPointsMax;
        RefillAP();
    }

    public void ApplyDamage(int val)
    {
        AdjustHitPoints(-val);
    }

    public void ApplyHealing(int val)
    {
        AdjustHitPoints(val);
    }

    public void AdjustHitPoints(int val)
    {
        _hitPointsCurrent += val;
        Debug.Log("HP After: " + _hitPointsCurrent);
    }

    public int Reaction
    {
        get
        {
            return _reaction;
        }
    }

    public int MaxAP
    {
        get
        {
            return _dexterity;
        }
    }

    public int CurrentAP
    {
        get { return _actionPointsCurrent; }
    }

    public void RefillAP()
    {
        _actionPointsCurrent = MaxAP;
        if (OnRemainingAPChanged != null)
        {
            OnRemainingAPChanged ();
        }
    }

    public void ModifyAP(int val)
    {
        if(_actionPointsCurrent + val < 0)
        {
            Debug.Log("Not enough AP remaining: " + _actionPointsCurrent);
            return;
        }
        _actionPointsCurrent += val;
        Debug.Log("Remaining AP: " + CurrentAP);
        if (OnRemainingAPChanged != null)
                        {
            OnRemainingAPChanged ();
        }
    }

    public int MaxJump
    {
        get
        {
            return Mathf.FloorToInt(_dexterity / 2);
        }
    }

    public int MagicalDamage
    {
        get
        {
            return _creativity;
        }
    }

    public int Level
    {
        get { return _level; }
    }

    public int CurrentXP
    {
        get { return _xp; }
    }
}
