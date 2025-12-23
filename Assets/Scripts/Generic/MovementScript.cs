using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {

	public const float moveSpeed = 5;
	bool isMoving = false;
	Vector3[] pathList = new Vector3[0];
	float startTime;
	int currentPoint;
	CharacterObject movingCharacter;

	//called by control script to initialize move values
	public void MoveToPoint(CharacterObject character, Vector3[] path){
		isMoving = true;
		pathList = path;
		currentPoint = 0;
		startTime = Time.time;
		movingCharacter = character;
	}
	
	//where the magic happens
	void Update(){
		//character is moving
		if(isMoving){
			float fracJourney = (Time.time - startTime) * moveSpeed; //calculate how far along our path we are
			if(currentPoint == pathList.Length - 1){ //at the end of the path
				isMoving = false;
				//alert the base script that the movement is over
//				movingCharacter.MovementComplete(pathList.Length - 1);
			}
			//when we reach the end of the lerp, we've reached the next square
			//go to next square in the list
			else if(fracJourney > 1){
				fracJourney = 1;
				movingCharacter.transform.position = Vector3.Lerp(pathList[currentPoint], pathList[currentPoint + 1], 1); //ensures we end in the middle of the square
				if(currentPoint != pathList.Length - 1){
					currentPoint++;
					startTime = Time.time;
				}
			}
			
			else{
				movingCharacter.transform.position = Vector3.Lerp(pathList[currentPoint], pathList[currentPoint + 1], fracJourney);
			}
		}
		
	}
	
}