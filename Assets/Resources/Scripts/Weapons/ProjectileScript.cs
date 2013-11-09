using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	
	Rigidbody rigid;
	bool canCollide = false;
	
	//set starting speed and direction
	public void Initialise(int angleDeg, Vector3 endPos){
		endPos += Vector3.up * 0.5f; //aim just above the ground in the required square
		float angle = angleDeg * Mathf.Deg2Rad;
		rigid = GetComponent<Rigidbody>();
		Vector3 forwardDir = new Vector3(endPos.x - transform.position.x, 0, endPos.z - transform.position.z);
		float distance = Mathf.Abs(forwardDir.magnitude);
		forwardDir = forwardDir.normalized;
		transform.LookAt(forwardDir);
		
		int height = Mathf.FloorToInt(endPos.y - transform.position.y);
		
		//calculate the forace required to hit the location
		float startVelocityMagnitude;
		startVelocityMagnitude = (0.5f*Physics.gravity.magnitude*Mathf.Pow(distance,2)) / (height - (distance * Mathf.Tan(angle)) * Mathf.Pow(Mathf.Cos(angle),2));
		startVelocityMagnitude = Mathf.Sqrt(Mathf.Abs(startVelocityMagnitude));
		Vector3 startVelocity = (forwardDir * (startVelocityMagnitude + 1) * Mathf.Cos(angle)) + (Vector3.up * Mathf.Sin(angle) * startVelocityMagnitude);		rigid.velocity = startVelocity;
		StartCoroutine(Cooldown());
	}
	
	//used to stop the arrow from destroying itself when first spawning
	IEnumerator Cooldown(){
		yield return new WaitForSeconds(0.1f);
		canCollide = true;
	}
	
	void OnCollisionEnter(Collision c){
	    //if a character is hit, do damage
		if(c.collider.tag == "NPC"){
			c.collider.SendMessage("TakeDamage", 1);
		}
		//destroy self after hitting something
		if(canCollide){
			Destroy(gameObject);
		}
	}
}
