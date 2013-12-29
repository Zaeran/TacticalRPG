using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {
	bool rotating = false;
	bool freeLook = false;
	const float smoothing = 0.2f;
	Quaternion endRot;
	Vector3 endRotEuler;
	GameObject followObject = null;
	Camera mainCam;
	
	void Start(){
		endRotEuler = transform.eulerAngles;
		endRot = Quaternion.Euler(endRotEuler);
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").camera;
	}
	
	public void SetFollow(GameObject g){
		followObject = g;
	}
	
	public void FreeLook(){
		freeLook = true;
	}
	
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.LeftControl)){
			freeLook = !freeLook;
		}
		
		if(freeLook){
			transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * Input.GetAxis("Mouse Y");
			transform.position += new Vector3(transform.right.x, 0, transform.right.z) * Input.GetAxis("Mouse X");
		}
		else if(followObject != null){
			transform.position = followObject.transform.position;
		}
		
		if(!rotating){
			if(Input.GetKeyDown(KeyCode.A)){
				rotating = true;
				endRotEuler = transform.eulerAngles + new Vector3(0,90,0);
			}
			if(Input.GetKeyDown(KeyCode.D)){
				rotating = true;
				endRotEuler = transform.eulerAngles - new Vector3(0,90,0);
			}
			if(Input.GetKeyDown(KeyCode.W)){
				mainCam.orthographicSize -= 1.5f;
			}
			if(Input.GetKeyDown(KeyCode.S)){
				mainCam.orthographicSize += 1.5f;
			}
		}
		
		endRot = Quaternion.Euler(endRotEuler);
		
		if(Mathf.Abs(transform.eulerAngles.y - endRot.eulerAngles.y) > 0.3f){
			transform.rotation = Quaternion.Lerp(transform.rotation, endRot, smoothing);
		}
		else if(rotating){
			transform.eulerAngles = endRot.eulerAngles;
			rotating = false;
		}
	}
}
