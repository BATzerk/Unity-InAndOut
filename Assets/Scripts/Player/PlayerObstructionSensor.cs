using UnityEngine;
using System.Collections;

public class PlayerObstructionSensor : MonoBehaviour {
	// References
	private Player playerRef; // set by player itself!
	public void SetPlayerRef(Player tempPlayerRef) { playerRef = tempPlayerRef; }
	// Properties
	private int numCurrentCollisions; // how many things I'm touching at this moment.
	public bool IsObstruction { get { return numCurrentCollisions > 0; } }

	void Start () {
		// Reset numCurrentCollisions
		numCurrentCollisions = 0;
	}


	void FixedUpdate() {
		numCurrentCollisions = 0;
	}
	void OnTriggerStay2D(Collider2D other) {
		if (DoCollideWithOther(other)) {
			numCurrentCollisions ++;
		}
	}
	/*
	void OnTriggerEnter2D(Collider2D other) {
		// NOT the player?
		if (DoCollideWithOther(other)) {
			numCurrentCollisions ++;
		}
		Debug.Log("Trigger EEENNTER!  " + numCurrentCollisions);
	}
	void OnTriggerExit2D(Collider2D other) {
		// NOT the player?
		if (DoCollideWithOther(other)) {
			numCurrentCollisions --;
		}
		Debug.Log("Trigger exit!  " + numCurrentCollisions);
	}
	*/

	private bool DoCollideWithOther(Collider2D other) {
		if (other.tag == "Player") return false; // Ignore the player.
		if (other.isTrigger) return false; // Ignore all triggers; solid objects only.
		if (other.gameObject.layer == playerRef.gameObject.layer) return false; // Ignore anything on my same colorID layer!
		if (playerRef.BoxHolding!=null && other.gameObject==playerRef.BoxHolding.gameObject) return false; // Ignore the box the player is holding.
		return true;
	}

	/*
	void FixedUpdate() {

	}
	void OnTriggerStay2D(Collider2D other) {
		// NOT the player?
		//		if (other.tag != "Player") {
		Debug.Log ("obstruction on some side");
		//		}
	}
	*/

}
