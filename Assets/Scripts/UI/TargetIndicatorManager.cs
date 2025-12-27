using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicatorManager : MonoBehaviour
{
    public static TargetIndicatorManager instance;

    public GameObject indicator;

    void Awake()
    {
        instance = this;
    }
    
    public void CreateTargetIndicators(List<ClickableTarget> targets)
    {
                //create indicators per target
        foreach(ClickableTarget t in targets)
        {
            Instantiate(indicator, t.transform.position + new Vector3(0,1,0), Quaternion.identity);
            //create indicator
        }
    }

    public static void RemoveIndicators()
    {
        TargetIndicator.RemoveIndicators();
    }
}
