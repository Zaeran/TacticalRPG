using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateSkillButton : MonoBehaviour
{
    string _mySkill;
    BattleUI _ui;
    public TextMeshProUGUI myText;

    public void Initialize(string skill, BattleUI ui)
    {
        _mySkill = skill;
        myText.text = skill;
        _ui = ui;
    }

    public void SelectSkill()
    {
        _ui.UseAction(_mySkill);
    }
}
