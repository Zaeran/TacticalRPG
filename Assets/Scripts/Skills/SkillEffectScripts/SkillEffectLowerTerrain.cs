using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectLowerTerrain : ISkillEffect
{

    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        DrawSquaresScript.DestroyValidSquares();
        foreach (ClickableTarget hitObject in hitCharacters)
        {
            bool isCharacterToMove = false;
            if(hitObject is TerrainObject)
            {
                hitObject.transform.parent.localScale += new Vector3(0, -0.5f, 0);
                if (hitObject.transform.parent.localScale.y < 0.5f)
                {
                    hitObject.transform.parent.localScale = new Vector3(1, 0.5f, 1);
                }
                RaycastHit hit;
                if(Physics.Raycast(hitObject.transform.parent.position + new Vector3(0,hitObject.transform.parent.localScale.y - 0.1f, 0), Vector3.up, out hit))
                {
                    Debug.Log("Hit name: " + hit.collider.gameObject.name);
                    ClickableTarget target = hit.collider.GetComponent<CharacterObject>();
                    if(target != null) //There's a character on the terrain
                    {
                        ((CharacterObject)target).Fall();
                    }
                }
            }
        }
    }

        public string Description(CharacterObject c)
    {
        return string.Format("Lower terrain height by 1");
    }
}