using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {

	public const float moveSpeed = 5;
	bool isMoving = false;
	int maxMovement = 0;
	Vector3[] pathList = new Vector3[0];
	float startTime;
	int currentPoint;
	int squaresMoved;
	GameObject origin;

	//called by control script to initialize move values
	public void MoveToPoint(GameObject Origin, Vector3[] path, int maxMove){
		isMoving = true;
		pathList = path;
		currentPoint = 0;
		startTime = Time.time;
		maxMovement = maxMove;
		squaresMoved = 0;
		origin = Origin;
	}
	
	//where the magic happens
	void Update(){
		//character is moving
		if(isMoving){
			float fracJourney = (Time.time - startTime) * moveSpeed; //calculate how far along our path we are
			if(currentPoint == pathList.Length - 1 || maxMovement <= 0){ //at the end of the path
				isMoving = false;
				//alert the base script that the movement is over
				origin.SendMessage("MovementOver", pathList.Length - 1);
			}
			//when we reach the end of the lerp, we've reached the next square
			//go to next square in the list
			else if(fracJourney > 1){
				fracJourney = 1;
				maxMovement--;
				origin.transform.position = Vector3.Lerp(pathList[currentPoint], pathList[currentPoint + 1], 1); //ensures we end in the middle of the square
				if(currentPoint != pathList.Length - 1){
					currentPoint++;
					squaresMoved++;
					startTime = Time.time;
				}
			}
			
			else{
				origin.transform.position = Vector3.Lerp(pathList[currentPoint], pathList[currentPoint + 1], fracJourney);
			}
		}
		
	}
	
}