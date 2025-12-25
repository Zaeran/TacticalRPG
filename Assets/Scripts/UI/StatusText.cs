using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusText : MonoBehaviour
{
    public static StatusText instance;
    public TextMeshProUGUI myText;


    private void Awake()
    {
        instance = this;
    }

    public static void SetStatusText(string text, float vanishTime = 0)
    {
        instance.SetText(text, vanishTime);
    }

    void SetText(string text, float vanishTime)
    {
        StopAllCoroutines();
        myText.text = text;
        if (vanishTime > 0)
        {
            StartCoroutine(ClearStatusText());
        }
    }

    IEnumerator ClearStatusText()
    {
        yield return new WaitForSeconds(2);
        myText.text = "";
    }
}
