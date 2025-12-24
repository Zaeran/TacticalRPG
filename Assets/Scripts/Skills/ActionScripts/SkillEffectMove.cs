using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectMove : ISkillEffect
{
    public void ProcessEffect(CharacterObject c, List<ClickableTarget> hitCharacters, Vector4 point)
    {
        List<Vector4> validPoints = FindValidPoints.GetPoints("Move", c.gameObject.transform.position, 20, c.MyCharacter.JumpStat);
        Vector3[] movePath = Pathfinding.StartPathFinding(point, validPoints, c.MyCharacter.JumpStat, c.gameObject);
        if (movePath.Length > 0)
        {
            DrawSquaresScript.DestroyValidSquares();
            c.GetComponent<MovementScript>().MoveToPoint(c, movePath);
        }
    }
}
