using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
        [System.Flags] public enum SkillTag { Movement, Attack, Physical, Magical, Buff, Debuff, Fire, Water, Earth, Air };
        string _name;
        SkillTag _tags;

        List<ISkillPrerequisite> _prereqs;
        ISkillTargeting _targeting;
        ISkillTargetRadius _targetRadius;
        List<ISkillCost> _skillCost;
        List<ISkillEffect> _effects;

        public event Vector4Event OnSkillTargeted;

        public Skill (string sName, SkillTag tags, ISkillTargeting targeting, List<ISkillEffect> effects)
        {
                _name = sName;
                _tags = tags;
                _prereqs = new List<ISkillPrerequisite>();
                _targeting = targeting;
                _effects = effects;
        }

        public string Name
                {
                get { return _name; }
        }

        public void AddPrerequisite(ISkillPrerequisite prereq)
    {
                _prereqs.Add(prereq);
    }

        public bool TestPrerequisites(CharacterObject c)
    {
                for(int i = 0; i < _prereqs.Count; i++) {
            if(!_prereqs[i].MeetsPrerequisite(c)) {
                                return false;
            }
        }
                return true;
    }

        public void StartSkillTargeting (CharacterObject c)
        {
                _targeting.SelectTarget (c);
                MouseControlScript.OnTileClicked += TargetSelected;
        }

        public void TargetSelected (Vector4 position)
        {
                MouseControlScript.OnTileClicked -= TargetSelected;
                if (OnSkillTargeted != null) {
                        OnSkillTargeted (position);
                }
        }

        public bool ProcessSkillEffect (CharacterObject c, Vector4 point)
        {
                bool canPayCost = false;
                for(int i = 0; i < _skillCost.Count; i++) {
                        canPayCost = canPayCost && _skillCost[i].CanPayCost(c, point);
        }
        if (!canPayCost) {
                        return false;
        }
                List<CharacterObject> hitCharacters = _targetRadius.GetTargets(c, point);
                for (int i = 0; i < _effects.Count; i++) {
                        _effects [i].ProcessEffect (c, hitCharacters, point);
                }
                return true;
        }

}
