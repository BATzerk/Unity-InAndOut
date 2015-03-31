using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	// Constants
	private const string STATE_FOLLOW_PLAYER = "FollowPlayer";
	//	private const string STATE_FOLLOW_NOTHING = "FollowNothing";
	// Settables
	private float playerCenterRange = 80; // in PIXELS. How much room is in the center of the screen where I won't move to follow the player? The RADIUS, not the diameter.
	// References (external)
	private Transform playerTransform;
	private Transform transformFollowing;
	// References (internal)
	private Camera camera;
	// Properties
	private const float posZ = -10;
	private string currentState;
	
	void Start () {
		// Set camera!
		camera = GetComponent<Camera> ();
	}
	
	public void Reset(Transform _playerTransform) {
		// Set playerTransform reference
		playerTransform = _playerTransform;
		transformFollowing = playerTransform;
		//		transform.position = new Vector3(playerTransform.position.x, transform.position.y, posZ);
		// Default to following the player.
		//		SetState(STATE_FOLLOW_PLAYER);
	}
	
	
	//	public void SetState(string newState) {
	//		currentState = newState;
	//
	//	}
	public void SetStateFromTriggerZone(GameObject targetGO, string state) {
		// Follow player?!
		if (state == "FollowPlayer") {
			transformFollowing = playerTransform;
		}
		// Follow something else?!
		else if (state == "TargetGO") {
			transformFollowing = targetGO.transform;
		}
		// Follow nothing??
		else if (state == "FollowNothing") {
			transformFollowing = null;
		}
	}
	
	
	void FixedUpdate () {
		// -- FOLLOW NOTHING --
		//		if (currentState == STATE_FOLLOW_NOTHING) {
		//
		//		}
		// -- FOLLOW PLAYER --
		//		if (currentState == STATE_FOLLOW_TRANSFORM) {
		if (transformFollowing != null) {
			float x = transform.position.x;
			float targetX = transformFollowing.position.x;
			float targetPosX = x;
			
			float centerRange = 0;
			float easeAmount = 60f;
			if (transformFollowing==playerTransform) {
				centerRange = playerCenterRange;
				easeAmount = 10f;
			}
			
			// Out of range LEFT?
			if (x < targetX - centerRange) {
				targetPosX = targetX - centerRange;
			}
			// Out of range RIGHT?
			else if (x > targetX + centerRange) {
				targetPosX = targetX + centerRange;
			}
			// IN range?
			else {
			}
			
			float posX = x + (targetPosX-x) / easeAmount;
			float posY = transform.position.y;
			transform.position = new Vector3(posX, posY, posZ);
		}
		//		}
	}
}






