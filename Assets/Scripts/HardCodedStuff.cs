using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardCodedStuff : MonoBehaviour
{
    public int APModification;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TurnController.CurrentCharacterTurn.UseAP(APModification);
        }
    }
}
