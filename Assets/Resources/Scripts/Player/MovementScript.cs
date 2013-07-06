using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	
	bool isMoving = false;
	Vector3[] pathList = new Vector3[0];
	public float moveSpeed;
	float startTime;
	int currentPoint;
	
	//called by control script to initialize move values
	public void MoveToPoint(Vector3[] path){
		isMoving = true;
		pathList = path;
		currentPoint = 0;
		startTime = Time.time;
	}
	
	//where the magic happens
	void Update(){
		
		if(isMoving){
			float fracJourney = (Time.time - startTime) * moveSpeed;
			if(currentPoint == pathList.Length - 1){
				isMoving = false;
			}
			
			else if(fracJourney > 1){
				fracJourney = 1;
				transform.position = Vector3.Lerp(pathList[currentPoint], pathList[currentPoint + 1], 1);
				if(currentPoint != pathList.Length - 1){
					currentPoint++;
					startTime = Time.time;
				}
			}
			
			else{
				transform.position = Vector3.Lerp(pathList[currentPoint], pathList[currentPoint + 1], fracJourney);
			}
		}
		
	}
	
}