using UnityEngine;
using System.Collections;

public class BoxObstructionSensor : MonoBehaviour {
	// References
	private Box boxRef; // set by player itself!
	public void SetBoxRef(Box tempBoxRef) { boxRef = tempBoxRef; }
	// Properties
	private int numCurrentCollisions; // how many things I'm touching at this moment.
	public bool IsObstruction { get { return numCurrentCollisions > 0; } }
	
	void Start () {
		// Reset numCurrentCollisions
		numCurrentCollisions = 0;
	}
	
	
	//*
	void FixedUpdate() {
		numCurrentCollisions = 0;
	}
	void OnTriggerStay2D(Collider2D other) {
		if (DoCollideWithOther(other)) {
			numCurrentCollisions ++;
		}
	}
	//*/
//	void OnTriggerEnter2D(Collider2D other) {
//		// NOT the player?
//		if (DoCollideWithOther(other)) {
//			numCurrentCollisions ++;
//		}
//	}
//	void OnTriggerExit2D(Collider2D other) {
//		// NOT the player?
//		if (DoCollideWithOther(other)) {
//			numCurrentCollisions --;
//		}
//	}
	
	private bool DoCollideWithOther(Collider2D other) {
		if (other.gameObject == boxRef.gameObject) return false; // Ignore my box!
		if (other.tag == "Player") return false; // Ignore the player.
		if (other.isTrigger) return false; // Ignore all triggers; solid objects only.
//		if (other.gameObject.layer == boxRef.gameObject.layer) return false; // Ignore anything on my same colorID layer!
		return true;
	}
}
