using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    static bool _ctrlHeld;
    static bool _shiftHeld;


    // Update is called once per frame
    void Update()
    {
        _ctrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
        _shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    public static bool CtrlHeld
    {
        get { return _ctrlHeld; }
    }

    public static bool ShiftHeld
    {
        get { return _shiftHeld; }
    }
}
