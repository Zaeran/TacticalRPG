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
        List<ISkillEffect> _effects;

        public event Vector4Event OnSkillTargeted;

        public Skill (string sName, SkillTag tags, ISkillTargeting targeting, List<ISkillEffect> effects)
        {
                _name = sName;
                _tags = tags;
                _targeting = targeting;
                _effects = effects;
        }

        public string Name
                {
                get { return _name; }
        }

        public int APCost {
                get { return _apCost; }
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
                bool stateChanged = false;
                for (int i = 0; i < _effects.Count; i++) {
                        stateChanged = stateChanged || _effects [i].ProcessEffect (c, point);
                }
                return stateChanged;
        }

}
