using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject obj;

    public void Toggled()
    {
        obj.SetActive(!obj.activeSelf);
    }
}
