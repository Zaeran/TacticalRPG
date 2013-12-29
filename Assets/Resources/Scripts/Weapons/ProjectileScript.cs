using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	
	Rigidbody rigid;
	bool canCollide = false;
	int wpnDamage;
	int skillCost;
	GameObject origin;
	
	//set starting speed and direction
	//aims for center of square
	public void Initialise(int angleDeg, Vector3 endPos, float projectileHeight, int damage, GameObject firedBy, int cost){
		origin = firedBy;
		skillCost = cost;
		wpnDamage = damage;
		collider.enabled = false;
		float angle = angleDeg * Mathf.Deg2Rad;
		rigid = GetComponent<Rigidbody>();
		Vector3 forwardDir = new Vector3(endPos.x - transform.position.x, 0, endPos.z - transform.position.z);
		float distance = Mathf.Abs(forwardDir.magnitude);
		forwardDir = forwardDir.normalized;
		transform.LookAt(forwardDir);
		
		float height = endPos.y - transform.position.y;
		
		//calculate the forace required to hit the location
		float startVelocityMagnitude;
		startVelocityMagnitude = ((0.5f * Physics.gravity.magnitude * Mathf.Pow(distance,2)) / (height - (distance * Mathf.Tan(angle)))) / Mathf.Pow(Mathf.Cos(angle),2);
		startVelocityMagnitude = Mathf.Sqrt(Mathf.Abs(startVelocityMagnitude));
		//xCos(angle), ySin(angle)
		Vector3 startVelocity = (forwardDir * (startVelocityMagnitude) * Mathf.Cos(angle)) + (Vector3.up * Mathf.Sin(angle) * startVelocityMagnitude);
		rigid.velocity = startVelocity;
		StartCoroutine(Cooldown());
	}
	
	//used to stop the arrow from destroying itself when first spawning
	IEnumerator Cooldown(){
		yield return new WaitForSeconds(0.2f);
		collider.enabled = true;
		canCollide = true;
	}
	
	void OnCollisionEnter(Collision c){
	    //if a character is hit, do damage
		if(canCollide){
			origin.SendMessage("ActionComplete", skillCost);
			if(c.collider.tag == "NPC" || c.collider.tag == "Player"){
				c.collider.SendMessage("TakeDamage", wpnDamage);
			}
			//destroy self after hitting something
			Destroy(gameObject);
		}
	}
}
