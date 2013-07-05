using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {
	bool rotating = false;
	const float smoothing = 0.2f;
	Quaternion endRot;
	Vector3 endRotEuler;
	
	void Start(){
		endRotEuler = transform.eulerAngles;
		endRot = Quaternion.Euler(endRotEuler);
	}	
	
	void Update () {
		if(!rotating){
			if(Input.GetKeyDown(KeyCode.A)){
				rotating = true;
				endRotEuler = transform.eulerAngles + new Vector3(0,90,0);
				Debug.Log(endRot.y);
			}
			if(Input.GetKeyDown(KeyCode.S)){
				rotating = true;
				endRotEuler = transform.eulerAngles - new Vector3(0,90,0);
			}
		}
		
		endRot = Quaternion.Euler(endRotEuler);
		
		if(Mathf.Sqrt(Mathf.Pow(transform.eulerAngles.y - endRot.eulerAngles.y,2)) > 0.3f){
			transform.rotation = Quaternion.Lerp(transform.rotation, endRot, smoothing);
		}
		else if(rotating){
			transform.eulerAngles = endRot.eulerAngles;
			rotating = false;
		}
	}
}
