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
	AIScript parentAIScript;
	
	void Start(){
		if(GetComponent<PlayerControlsScript>()){
			isPlayer = true;
			parentScript = GetComponent<PlayerControlsScript>();
		}
		else if(GetComponent<AIScript>()){
			isPlayer = false;
			parentAIScript = GetComponent<AIScript>();
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
		//character is moving
		if(isMoving){
			float fracJourney = (Time.time - startTime) * moveSpeed; //calculate how far along our path we are
			if(currentPoint == pathList.Length - 1 || maxMovement <= 0){ //at the end of the path
				isMoving = false;
				//alert the base script that the movement is over
				if(isPlayer){
					parentScript.StopMovingConfirmation();
				}
				else{
					parentAIScript.StopMovingConfirmation();
				}
			}
			//when we reach the end of the lerp, we've reached the next square
			//go to next square in the list
			else if(fracJourney > 1){
				fracJourney = 1;
				maxMovement--;
				transform.position = Vector3.Lerp(pathList[currentPoint], pathList[currentPoint + 1], 1); //ensures we end in the middle of the square
				if(currentPoint != pathList.Length - 1){
					currentPoint++;
					startTime = Time.time;
				}
			}
			
			else{
				transform.position = Vector3.Lerp(pathList[currentPoint], pathList[currentPoint + 1], fracJourney);
				//add animation here
			}
		}
		
	}
	
}