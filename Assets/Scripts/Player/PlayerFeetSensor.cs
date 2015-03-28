using UnityEngine;
using System.Collections;

public class PlayerFeetSensor : MonoBehaviour {
	// Properties
	private int numCurrentCollisions; // how many things I'm touching at this moment.
	// Getters
	public bool IsGrounded {
		get { return numCurrentCollisions > 0; }
	}


	void Start () {
		numCurrentCollisions = 0;
	}

	void FixedUpdate() {
//		Debug.Log ("Fixed update isGrounded  " + isGrounded);
//		isGrounded = false;
	}
	
	void OnTriggerStay2D(Collider2D other) {
//		Debug.Log ("OnTriggerStay2D!");
//		isGrounded = true;
	}
	void OnTriggerEnter2D(Collider2D other) {
		// Ignore certain collisions
		if (other.tag == "CameraTriggerZone") { return; }
		numCurrentCollisions ++;
	}
	void OnTriggerExit2D(Collider2D other) {
		// Ignore certain collisions
		if (other.tag == "CameraTriggerZone") { return; }
		numCurrentCollisions --;
	}
}
