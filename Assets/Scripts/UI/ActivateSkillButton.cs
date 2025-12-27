using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateSkillButton : MonoBehaviour
{
    Skill _mySkill;
    BattleUI _ui;
    public TextMeshProUGUI myText;

    public void Initialize(Skill skill, BattleUI ui)
    {
        _mySkill = skill;
        myText.text = skill.Name;
        _ui = ui;
    }

    public void SelectSkill()
    {
        _ui.UseAction(_mySkill.Name);
    }

    public void ShowSkillDescription()
    {
        _ui.SetSkillDescriptionText(_mySkill.SkillDescription());
    }

    public void HideSkillDescriptionText()
    {
        _ui.SetSkillDescriptionText("");
    }
}
