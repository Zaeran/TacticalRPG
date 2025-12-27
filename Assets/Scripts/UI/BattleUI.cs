using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{

    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characteAPText;
        public TextMeshProUGUI characterHPText;
    public TextMeshProUGUI characterEquipmentText;
    public GameObject skillObject;
    public GameObject skillButtonPrefab;
        public GameObject weaponButtonPrefab;

    public TextMeshProUGUI skillDescriptionText;

    private void Awake()
    {
        TurnController.OnCharacterTurnEnding += CharacterTurnEnding;
        TurnController.OnNextCharacterTurn += CharacterTurnStarted;
    }

    public void PassTurn()
    {
        //TurnController.TurnOver();
        TurnController.CurrentCharacterTurn.StartSetFacingDirection();
    }

    public void UseAction(string action)
    {
        TurnController.CurrentCharacterTurn.SetAction(action);
    }

    public void CancelAction()
    {
        TurnController.CurrentCharacterTurn.CancelSkill();
    }

    void CharacterTurnStarted(Character c)
    {
        characterNameText.text = c.CharacterName;
        CharacterAPChanged();
        CharacterHPCHanged();
        CharacterWeaponChanged();
        c.Attributes.OnRemainingAPChanged += CharacterAPChanged;
        c.Attributes.OnRemainingHPChanged += CharacterHPCHanged;
        c.OnWeaponChanged += CharacterWeaponChanged;
        skillObject.SetActive(false);
    }

    void CharacterTurnEnding(Character c)
    {
        c.Attributes.OnRemainingAPChanged -= CharacterAPChanged;
        c.Attributes.OnRemainingHPChanged -= CharacterHPCHanged;
        c.OnWeaponChanged -= CharacterWeaponChanged;
    }

    void CharacterAPChanged()
    {
        characteAPText.text = "AP: " + TurnController.CurrentCharacterTurn.MyCharacter.Attributes.CurrentAP + "/" + TurnController.CurrentCharacterTurn.MyCharacter.Attributes.MaxAP;
        skillObject.SetActive(false);
    }

    void CharacterHPCHanged()
    {
        characterHPText.text = "HP: " + TurnController.CurrentCharacterTurn.MyCharacter.Attributes.HealthCurrent + "/" + TurnController.CurrentCharacterTurn.MyCharacter.Attributes.HealthMax;
    }

    void CharacterWeaponChanged()
    {
        characterEquipmentText.text = "Weapon: " + TurnController.CurrentCharacterTurn.MyCharacter.Weapon.Name;
        skillObject.SetActive(false);
        SetSkillDescriptionText("");
    }

    public void OpenSkillList()
    {
        skillObject.SetActive(true);
        LoadAvailableSkills();
    }
    public void OpenWeaponList()
    {
        skillObject.SetActive(true);
        LoadAvailableWeapons();
    }

    void LoadAvailableSkills()
    {
        UIUtilities.ClearChildren(skillObject.transform);
        List<Skill> allowedSkills = new List<Skill>();
        foreach(Skill s in AllSkills.GetAllSkills)
        {
            if(s.Name == "Move")
            {
                continue;
            }
            if (s.CanUseWithSkillTrees(TurnController.CurrentCharacterTurn.MyCharacter.ActiveSkillTrees))
            {
                allowedSkills.Add(s);
            }
        }

        foreach(Skill s in allowedSkills)
        {
            GameObject btn = Instantiate(skillButtonPrefab, skillObject.transform);
            btn.GetComponent<ActivateSkillButton>().Initialize(s, this);
        }
    }

    public void SetSkillDescriptionText(string text)
    {
        skillDescriptionText.text = text;
    }

    void LoadAvailableWeapons()
    {
        UIUtilities.ClearChildren(skillObject.transform);
        List<EquipmentWeapon> allowedSkills = new List<EquipmentWeapon>();
        foreach(EquipmentWeapon weapon in AllWeapons.GetAllWeapons)
        {
            GameObject btn = Instantiate(weaponButtonPrefab, skillObject.transform);
            btn.GetComponent<WeaponSelectionButton>().Initialize(weapon, this);
        }
    }
}
