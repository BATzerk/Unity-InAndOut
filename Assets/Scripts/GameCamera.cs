using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	// Settables
	private float playerCenterRange = 80; // in PIXELS. How much room is in the center of the screen where I won't move to follow the player? The RADIUS, not the diameter.
	// References
	private Transform playerTransform;
	private Camera camera;
	// Properties
	private const float posZ = -10;

	void Start () {
		// Set camera!
		camera = GetComponent<Camera> ();
	}

	public void Reset(Transform _playerTransform) {
		playerTransform = _playerTransform;
		transform.position = new Vector3(playerTransform.position.x, transform.position.y, posZ);
	}
	
	void FixedUpdate () {
		// Update position from player!
		if (playerTransform != null) {
			float x = transform.position.x;
			float playerX = playerTransform.position.x;
			float targetPosX = x;
			// Out of range LEFT?
			if (x < playerX - playerCenterRange) {
				targetPosX = playerX - playerCenterRange;
			}
			// Out of range RIGHT?
			else if (x > playerX + playerCenterRange) {
				targetPosX = playerX + playerCenterRange;
			}
			// IN range?
			else {
			}

			float posX = x + (targetPosX-x) / 10f;
			float posY = transform.position.y;
			transform.position = new Vector3(posX, posY, posZ);
		}
	}
}
