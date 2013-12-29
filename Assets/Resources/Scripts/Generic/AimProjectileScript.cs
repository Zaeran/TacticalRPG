using UnityEngine;
using System.Collections;

public class AimProjectileScript : MonoBehaviour {
			
	public int Aim(Vector3 endPos){
		endPos += (Vector3.up * 0.5f); //replace 0.5f with half height of NPC/character model 
		Vector3 aimDirection = new Vector3(transform.position.x - endPos.x, 0, transform.position.z - endPos.z).normalized; //sets the direction to aim in on a 2D plane
		float elevationAngle = Vector3.Angle(aimDirection, transform.position - endPos); //gets the angle of elevation
		int aimAngle;
		if(transform.position.y > endPos.y){ //target is below us
			aimAngle = 45 - Mathf.FloorToInt(elevationAngle / 2);
		}
		else{
			aimAngle = 0;
		}
		return aimAngle;
	}
	
}
