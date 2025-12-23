using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectWeaponAttack : ISkillEffect
{
    public bool PayCost(CharacterObject c, int apCost)
    {
        if (c.MyCharacter.Attributes.CurrentAP < apCost)
        {
            return false;
        }
        c.MyCharacter.Attributes.ModifyAP(-apCost);
        return true;
    }

    public bool ProcessEffect(CharacterObject c, Vector4 point)
    {
        //Ensure a valid target
        RaycastHit hit;
        if (!Physics.Raycast(new Vector3(point.x, point.y + 1, point.z), Vector3.down, out hit))
        {
            return false;
        }
        CharacterObject hitCharacter = hit.collider.GetComponent<CharacterObject>();
        if(hitCharacter == null)
        {
            return false;
        }

        int apCost = 2;

        if (!PayCost(c, apCost))
        {
            return false;
        }
       
        DrawSquaresScript.DestroyValidSquares();
        hitCharacter.MyCharacter.Attributes.ApplyDamage(c.MyCharacter.Weapon.Damage);
        FloatingNumbersScript.CreateFloatingNumber(hitCharacter.transform.position, c.MyCharacter.Weapon.Damage.ToString(), Color.red);
        return true;
    }
}
