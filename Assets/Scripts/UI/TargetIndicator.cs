using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
   static event VoidEvent OnTimeToGo;

    void Start()
    {
     OnTimeToGo += Remove;   
    }

    void Remove()
    {
         OnTimeToGo += Remove;   
        Destroy(gameObject);
    }

    public static void RemoveIndicators()
    {
        if(OnTimeToGo != null)
        {
            OnTimeToGo();
        }
    }
}
