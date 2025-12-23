using UnityEngine;
using System.Collections;

public class FloatingNumbersScript : MonoBehaviour {


	string floatingText = "";
	Vector3 position = Vector3.zero;
	Vector3 screenPos = Vector3.zero;

	void Update(){
		if(floatingText != ""){
			screenPos = new Vector3(Camera.main.WorldToScreenPoint(position).x, Screen.height - Camera.main.WorldToScreenPoint(position).y, 0);
			position += Vector3.up * 0.03f;
		}
	}

	public void SetText(string text, Vector3 pos){
		floatingText = text;
		position = pos;
		StartCoroutine(FloatTimer());
	}

	IEnumerator FloatTimer(){
		yield return new WaitForSeconds(2);
		floatingText = "";
	}

	void OnGUI(){
		if(floatingText != ""){
			GUI.color = Color.red;
			GUI.Label(new Rect(screenPos.x, screenPos.y, 50,30), floatingText);
		}
	}
}
