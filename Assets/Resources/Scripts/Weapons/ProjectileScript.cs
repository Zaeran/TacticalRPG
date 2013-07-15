using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	
	Rigidbody rigid;
	
	public void Initialise(int angleDeg, Vector3 endPos){
		endPos += Vector3.up * 0.5f;
		float angle = angleDeg * Mathf.Deg2Rad;
		rigid = GetComponent<Rigidbody>();
		Vector3 forwardDir = new Vector3(endPos.x - transform.position.x, 0, endPos.z - transform.position.z);
		float distance = Mathf.Abs(forwardDir.magnitude);
		forwardDir = forwardDir.normalized;
		transform.LookAt(forwardDir);
		
		int height = Mathf.FloorToInt(endPos.y - transform.position.y);
		
		float startVelocityMagnitude;
		startVelocityMagnitude = (0.5f*Physics.gravity.magnitude*Mathf.Pow(distance,2)) / (height - (distance * Mathf.Tan(angle)) * Mathf.Pow(Mathf.Cos(angle),2));
		startVelocityMagnitude = Mathf.Sqrt(Mathf.Abs(startVelocityMagnitude));
		Vector3 startVelocity = (forwardDir * (startVelocityMagnitude + 1) * Mathf.Cos(angle)) + (Vector3.up * Mathf.Sin(angle) * startVelocityMagnitude);		rigid.velocity = startVelocity;
	}
}
