using UnityEngine;
using System.Collections;

public class PersistentObject : MonoBehaviour {

	// Use this for initialization
	void Start(){
		DontDestroyOnLoad(gameObject);
	}
}
