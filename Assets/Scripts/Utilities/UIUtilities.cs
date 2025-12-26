using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtilities
{
    public static void ClearChildren(Transform t)
    {
        for(int i = t.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(t.GetChild(i).gameObject);
        }
    }
}
