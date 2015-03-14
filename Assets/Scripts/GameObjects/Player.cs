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
	Rigidbody2D rigidbody; // My physics body
	BoxCollider2D feetSensor; // The sensor at my "feet" that determines if I'm on ground.
	GameObject bodySprites; // All images that'll be scaled by the direction I'm facing are in here.
	GameObject DEBUG_bodySprite; // JUST the body
	// Properties
	bool isGrounded = true;
	int directionFacing = 1; // Where I'm facing. -1 is left and 1 is right. It determines my X scale. No other values should be used.

	void Start () {
		// Identify components!
		rigidbody = (GetComponent<Rigidbody2D> ());
		foreach (Transform t in transform) {
			if (t.name == "BodySprites") bodySprites = t.gameObject;
			else if (t.name == "FeetSensor") feetSensor = t.GetComponent<BoxCollider2D>();
		}

		DEBUG_bodySprite = GameObject.Find ("Body");

		rigidbody.mass = 0.2f;
	}

	void FixedUpdate () {
		ApplyGravity ();
		SetIsGrounded ();
		InputLogic ();
		ApplyFriction ();
		TerminalVelocity ();

		if (isGrounded) DEBUG_bodySprite.renderer.material.color = Color.blue;
		else DEBUG_bodySprite.renderer.material.color = Color.cyan;

		Debug.Log ("Fixed update isGrounded  " + isGrounded);
		isGrounded = false;
	}
	
	void OnTriggerStay2D(Collider2D other) {
		Debug.Log ("OnTriggerStay2D!");
		isGrounded = true;
	}
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Trigger ENTERR");
	}
	void OnTriggerExit2D(Collider2D other) {
		Debug.Log ("Trigger EXXXXXXITT");
	}



	void ApplyGravity() {
		if (!isGrounded) {
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y+GRAVITY_FORCE);
		}
	}
	void SetIsGrounded() {
		//feetSensor.collider2D.
	}
	void InputLogic() {
		float inputX = Input.GetAxis ("Horizontal");

		// VELOCITY
		//	Horizontal
		Vector3 newVelocity = new Vector2(rigidbody.velocity.x+inputX*movementSpeed, rigidbody.velocity.y);
		rigidbody.velocity = newVelocity;
		//	Jump
		if (Input.GetButtonDown ("Jump")) {
			Jump();
		}

		// DIRECTION
		//	Update directionFacing if the horizontal input is significant enough!
		if (Mathf.Abs (inputX) > 0.05f) {
			directionFacing = Sign(inputX);
		}
		//	Apply scale!
		bodySprites.transform.localScale = new Vector2(directionFacing, bodySprites.transform.localScale.y);
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
		// If I'm on the ground, then jump!
		if (isGrounded) {
			rigidbody.velocity = new Vector2 (rigidbody.velocity.x, rigidbody.velocity.y - JUMP_FORCE);
		}
	}




	int Sign(float value) {
		if (value < 0) return -1;
		if (value > 0) return 1;
		return 0;
	}
}








