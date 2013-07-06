using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	
	bool isMoving = false;
	bool isPlayer = false;
	int maxMovement = 0;
	Vector3[] pathList = new Vector3[0];
	public float moveSpeed;
	float startTime;
	int currentPoint;
	
	PlayerControlsScript parentScript;
	
	void Start(){
		if(GetComponent<PlayerControlsScript>()){
			isPlayer = true;
			parentScript = GetComponent<PlayerControlsScript>();
		}
	}
	
	//called by control script to initialize move values
	public void MoveToPoint(Vector3[] path, int maxMove){
		isMoving = true;
		pathList = path;
		currentPoint = 0;
		startTime = Time.time;
		maxMovement = maxMove;
	}
	
	//where the magic happens
	void Update(){
		
		if(isMoving){
			float fracJourney = (Time.time - startTime) * moveSpeed;
			if(currentPoint == pathList.Length - 1 || maxMovement <= 0){
				isMoving = false;
				if(isPlayer){
					parentScript.StopMovingConfirmation();
				}
			}
			
			else if(fracJourney > 1){
				fracJourney = 1;
				maxMovement--;
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