using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingNumber : MonoBehaviour
{
    Color myColour;
    TextMeshProUGUI myText;

    public void Initialize(string val, Color c)
    {
        myColour = c;
        myText = GetComponent<TextMeshProUGUI>();
        myText.text = val;
    }

    private void Update()
    {
        transform.position += new Vector3(0, 10 * Time.deltaTime, 0);
        myText.color = new Color(myColour.r, myColour.g, myColour.b, myText.color.a - Time.deltaTime / 2);
        if(myText.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
