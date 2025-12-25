using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtilities : MonoBehaviour
{
   public static Vector2 NearestStraightLineVector(Vector2 input)
    {
        float angle = Vector2.SignedAngle(Vector2.up, input);
        if(Mathf.Abs(angle) < 45) //forward
        {
            return Vector2.up;
        }
        else if(Mathf.Abs(angle) > 135)
        {
            return Vector2.down;
        }
        else if(angle < 0)
        {
            return Vector2.right;
        }
        else
        {
            return Vector2.left;
        }
        
    }
}
