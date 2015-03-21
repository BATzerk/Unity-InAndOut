﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// ================================
	//	Properties
	// ================================
	// Settables
	float movementSpeedAir = 20f;
	float movementSpeedGround = 200f;
	float maxVelX = 300;
	float maxVelY = 1000;
	float frictionGround = 0.8f;
	float JUMP_FORCE = -600;
	float GRAVITY_FORCE = -26;
	private const float MAX_ROTATION = 45f; // how far my body can rotate. We don't want a strict fixed rotation at 0-- leeway is nice.
	// Constants
	const KeyCode KEYCODE_BOX_GRAB = KeyCode.LeftShift;
	// Self-References
	GameObject bodyGO; // Everything I will flip horizontally (for direction facing) will be in here.
	Rigidbody2D rigidbody; // My physics body
	PlayerFeetSensor feetSensor; // The sensor at my "feet" that determines if I'm on ground.
	PlayerHandSensor handSensor; // The sensor at my "hands" that determines if I can grab something in front of me.
	PlayerObstructionSensor obstSensorL; // Left obstruction sensor
	PlayerObstructionSensor obstSensorR; // Right obstruction sensor
	GameObject bodySprite; // JUST the body image. Debug currently, basically.
	// External References
	Box boxHolding;
	// Properties
	float bodyWidth; // it's exactly how wide the sprite is! Currently used for offseting boxes' positions.
	int colorID = -1;
	int directionFacing = 1; // Where I'm facing. -1 is left and 1 is right. It determines my X scale. No other values should be used.

	// Getters (Private)
	private bool IsHoldingBox { get { return boxHolding != null; } }
	private bool IsObstructionL { get { return (boxHolding!=null && boxHolding.IsObstructionL); } }//return obstSensorL.IsObstruction || 
	private bool IsObstructionR { get { return (boxHolding!=null && boxHolding.IsObstructionR); } }//return obstSensorR.IsObstruction || 
	// Getters (Public)
	public Box BoxHolding { get { return boxHolding; } }
	public float BodyWidth { get { return bodyWidth; } }
	public Rigidbody2D MyRigidbody { get { return rigidbody; } }

	
	public void SetColorID(int newColorID) {
		colorID = newColorID;
		SetLayerRecursively(this.gameObject, newColorID);
		bodySprite.renderer.material.color = Colors.GetLayerColor(colorID);
	}
	private void SetLayerRecursively(GameObject go, int newLayer) {
		go.layer = newLayer;
//		foreach (Transform childTransform in go.transform) {
//			SetLayerRecursively(childTransform.gameObject, newLayer);
//		}
	}
	
	
	// ================================
	//	Start
	// ================================
	void Start () {
	}
	public void Reset() {
		// Identify components!
		rigidbody = (GetComponent<Rigidbody2D> ());
		// Loop through EVERY child of every child, associating references by name!
		IdentifyComponentsRecursively(transform);

		bodyWidth = bodySprite.renderer.bounds.size.x;
		obstSensorL.SetPlayerRef(this);
		obstSensorR.SetPlayerRef(this);

		rigidbody.mass = 0.2f;
		
		// Set initial values
		SetBoxHolding (null);
	}
	private void IdentifyComponentsRecursively(Transform t) {
		if (t.name == "Body") bodyGO = t.gameObject;
		else if (t.name == "BodySprite") bodySprite = t.gameObject;
		else if (t.name == "FeetSensor") feetSensor = t.GetComponent<PlayerFeetSensor>();
		else if (t.name == "HandSensor") handSensor = t.GetComponent<PlayerHandSensor>();
		else if (t.name == "ObstructionSensorL") obstSensorL = t.GetComponent<PlayerObstructionSensor>();
		else if (t.name == "ObstructionSensorR") obstSensorR = t.GetComponent<PlayerObstructionSensor>();
		// Do it again recursively!
		foreach (Transform childTransform in t) {
			IdentifyComponentsRecursively(childTransform);
		}
	}
	
	
	// ================================
	//	Update
	// ================================
	void Update() {
		InputLogicMovement ();
		InputLogicGrabbingBoxes ();
	}
	
	// ================================
	//	Input Functions
	// ================================
	void InputLogicMovement() {
		// Get inputX-- and if we have any obstructions, then modify it to 0 appropriately!
		float inputX = Input.GetAxis ("Horizontal");
		if (IsObstructionL && inputX<0) { inputX = 0; }
		if (IsObstructionR && inputX>0) { inputX = 0; }
		
		// VELOCITY
		//	Horizontal
		float movementSpeed = feetSensor.IsGrounded ? movementSpeedGround : movementSpeedAir;
		Vector3 newVelocity = new Vector2(rigidbody.velocity.x+inputX*movementSpeed, rigidbody.velocity.y);
		rigidbody.velocity = newVelocity;
		//	Jump
		if (Input.GetButtonDown ("Jump")) {
			Jump();
		}
		
		// DIRECTION
		//	Holding box?!
		if (IsHoldingBox) {
			// directionFacing will be respective to the box being left/right of me.
			directionFacing = Sign (boxHolding.MyRigidbody.position.x - rigidbody.position.x);
		}
		//	NOT holding box...
		else {
			//	Update directionFacing if the horizontal input is significant enough!
			if (Mathf.Abs (inputX) > 0.05f) {
				directionFacing = Sign(inputX);
			}
		}
		//	Apply scale for direction!
		bodyGO.transform.localScale = new Vector2(directionFacing, this.transform.localScale.y);
	}

	void Jump() {
		// If I'm on the ground, AND I'm not holding a box...
		if (feetSensor.IsGrounded && boxHolding==null) {
			// JUMP!
			rigidbody.velocity = new Vector2 (rigidbody.velocity.x, rigidbody.velocity.y - JUMP_FORCE);
		}
	}

	void InputLogicGrabbingBoxes() {
		// NOT holding a box...
		if (boxHolding == null) {
			// NEXT to a box and just hit GRAB key?!
			if (handSensor.BoxTouching!=null && Input.GetKeyDown(KEYCODE_BOX_GRAB)) {
				SetBoxHolding(handSensor.BoxTouching);
			}
		}
		// YES holding a box!
		else {
			// Just RELEASED GRAB key?!
			if (Input.GetKeyUp(KEYCODE_BOX_GRAB)) {
				SetBoxHolding(null);
			}
		}
	}


	
	// ================================
	//	Fixed Update
	// ================================
	void FixedUpdate () {
		ApplyGravity ();
		ApplyFriction ();
		HaltForObstructions ();
		TerminalVelocity ();
		LimitRotation ();
	}

	
	// ================================
	//	Physics-themed Methods
	// ================================
	void ApplyGravity() {
		//if (!isGrounded) {
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y+GRAVITY_FORCE);
		//}
	}
	void ApplyFriction() {
		// If I'm on the ground, apply basic ground friction!
		if (feetSensor.IsGrounded) {
			rigidbody.velocity = new Vector2(rigidbody.velocity.x*frictionGround, rigidbody.velocity.y);
		}
	}
	void HaltForObstructions() {
		// If I've got left/right obstructions, stop my velocity in that direction!
		if (IsObstructionL && rigidbody.velocity.x<0) { rigidbody.velocity = new Vector2(0, rigidbody.velocity.y); }
		if (IsObstructionR && rigidbody.velocity.x>0) { rigidbody.velocity = new Vector2(0, rigidbody.velocity.y); }
	}
	void TerminalVelocity() {
		// Limit how fast I can move.
		rigidbody.velocity = new Vector2(
			Mathf.Clamp (rigidbody.velocity.x, -maxVelX,maxVelX),
			Mathf.Clamp (rigidbody.velocity.y, -maxVelY,maxVelY)
		);
	}
	void LimitRotation() {
		// Limit how far I can rotate my rigidbody.
		rigidbody.rotation = Mathf.Clamp (rigidbody.rotation, -MAX_ROTATION,MAX_ROTATION);
		rigidbody.angularVelocity *= 0.7f;
		rigidbody.angularVelocity += (-rigidbody.rotation * 2);
	}


	// ================================
	//	Boxes
	// ================================
	void SetBoxHolding(Box tempBox) {
		// FIRST, if I'm already HOLDING a box, tell it it's ungrabbed!
		if (boxHolding != null) {
			boxHolding.OnUngrabbed ();
		}
		// Set new boxHolding!
		boxHolding = tempBox;
		// Notify boxHolding that it's been grabbed!
		if (boxHolding != null) {
			boxHolding.OnGrabbed (this);
		}
	}

	
	
	// ================================
	//	Utility Functions
	// ================================
	int Sign(float value) {
		if (value < 0) return -1;
		if (value > 0) return 1;
		return 0;
	}
}








