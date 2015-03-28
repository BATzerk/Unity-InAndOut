using UnityEngine;
using System.Collections;

public class CameraTriggerZone : MonoBehaviour {
	// References (external)
	private GameCamera gameCameraRef;
	// Properties
	public GameObject TargetGOOnEnter; // if this is null, nothing will happen when we enter this zone
	public GameObject TargetGOOnExit; // if this is null, nothing will happen when we exit this zone
	public string StateOnEnter = ""; // if this is empty (or null), I won't do anything when the player enters me.
	public string StateOnExit = ""; // if this is empty (or null), I won't do anything when the player exits me.

	void Start() {
		// Find references
		gameCameraRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameCamera>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Player") { return; } // Not the player? Ignore it.
//		if (StateOnEnter==null || StateOnEnter.Length <= 0) { return; } // I don't have a command for this? Don't do anything.
		gameCameraRef.SetStateFromTriggerZone(TargetGOOnEnter, StateOnEnter);
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag != "Player") { return; } // Not the player? Ignore it.
//		if (StateOnExit==null || StateOnExit.Length <= 0) { return; } // I don't have a command for this? Don't do anything.
		gameCameraRef.SetStateFromTriggerZone(TargetGOOnExit, StateOnExit);
	}
}
