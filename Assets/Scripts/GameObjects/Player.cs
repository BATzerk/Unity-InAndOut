using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// Settables
	float movementSpeed = 200f;
	float maxVelX = 300;
	float maxVelY = 1000;
	float frictionGround = 0.8f;
	float JUMP_FORCE = -600;
	float GRAVITY_FORCE = -26;
	// References
	Rigidbody2D rigidbody;
	// Properties
	bool isGrounded = true;

	void Start () {
		// Identify components!
		rigidbody = (GetComponent<Rigidbody2D> ());
	}

	void Update () {
		ApplyGravity ();
		InputLogic ();
		ApplyFriction ();
		TerminalVelocity ();
	}



	void ApplyGravity() {
//		if (isGrounded) {
//
//		}
		rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y+GRAVITY_FORCE);
	}
	void InputLogic() {
		// Accept player input to influence my velocity.
		// Horizontal
		float inputX = Input.GetAxis ("Horizontal");
		Vector3 newVelocity = new Vector2(rigidbody.velocity.x+inputX*movementSpeed, rigidbody.velocity.y);
		rigidbody.velocity = newVelocity;
		// Jump
		if (Input.GetButtonDown ("Jump")) {
			Jump();
		}
	}
	void ApplyFriction() {
		// If I'm on the ground, apply basic ground friction!
		if (isGrounded) {
			rigidbody.velocity = new Vector2(rigidbody.velocity.x*frictionGround, rigidbody.velocity.y);
		}
	}
	void TerminalVelocity() {
		// Limit how fast I can move.
		rigidbody.velocity = new Vector2(
			Mathf.Clamp (rigidbody.velocity.x, -maxVelX,maxVelX),
			Mathf.Clamp (rigidbody.velocity.y, -maxVelY,maxVelY)
		);
	}

	void Jump() {
		rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y-JUMP_FORCE);
	}
}








