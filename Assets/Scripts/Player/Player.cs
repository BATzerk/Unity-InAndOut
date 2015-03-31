using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// ================================
	//	Properties
	// ================================
	// Constants
	private const float movementSpeedAir = 20f;
	private const float movementSpeedGround = 200f;
	private const float maxVelX = 300;
	private const float maxVelY = 99999;
	private const float frictionGround = 0.8f;
	private const float JUMP_FORCE = -600;
	private const float MAX_ROTATION = 45f; // how far my body can rotate. We don't want a strict fixed rotation at 0-- leeway is nice.
	private const KeyCode KEYCODE_BOX_GRAB = KeyCode.LeftShift;
	// References (external)
	Box boxHolding;
	// References (internal)
	GameObject bodyGO; // Everything I will flip horizontally (for direction facing) will be in here.
	Rigidbody2D rigidbody; // My physics body
	PlayerFeetSensor feetSensor; // The sensor at my "feet" that determines if I'm on ground.
	PlayerHandSensor handSensor; // The sensor at my "hands" that determines if I can grab something in front of me.
	PlayerObstructionSensor obstSensorL; // Left obstruction sensor
	PlayerObstructionSensor obstSensorR; // Right obstruction sensor
	SpriteRenderer bodySprite; // JUST the body image. For coloring, AND for HACK/PLACEHOLDER determining my physics width/height.
	SpriteRenderer headSprite; // JUST the head. For coloring.
	// Properties
	float bodyWidth; // it's exactly how wide the sprite is! Currently used for offseting boxes' positions.
	float bodyHeight; // it's exactly how TALL the player is. Currently used for platform detection.
	int colorID = 0;
	int directionFacing = 1; // Where I'm facing. -1 is left and 1 is right. It determines my X scale. No other values should be used.
	Spring springTouching; // whatever spring I'm currently touching. This is set by the SPRING, not by me! (I want to keep as much code out of this class as I can.)
	Platform matchingPlatformTouching; // this is the platform I'm on that's the same colorID as I am.

	// Getters (Private)
	private bool IsTouchingSpring { get { return springTouching != null; } }
	private bool IsObstructionL { get { return (boxHolding!=null && boxHolding.IsObstructionL); } }//return obstSensorL.IsObstruction || 
	private bool IsObstructionR { get { return (boxHolding!=null && boxHolding.IsObstructionR); } }//return obstSensorR.IsObstruction || 
	// Getters (Public)
	public bool IsHoldingBox { get { return boxHolding != null; } }
	public Box BoxHolding { get { return boxHolding; } }
	public float BodyWidth { get { return bodyWidth; } }
	public float BodyHeight { get { return bodyHeight; } }
	public Rigidbody2D MyRigidbody { get { return rigidbody; } }
	public Spring SpringTouching { get { return springTouching; } set { springTouching = value; } }

	
	public void SetColorID(int newColorID) {
		colorID = newColorID;
		SetLayerRecursively(this.gameObject, WorldProperties.RigidbodyLayer(colorID));
		bodySprite.material.color = Colors.GetLayerColor(colorID);
		headSprite.material.color = Colors.GetLayerColor(colorID);
	}
	private void SetLayerRecursively(GameObject go, int newLayer) {
		go.layer = newLayer;
		foreach (Transform childTransform in go.transform) {
			SetLayerRecursively(childTransform.gameObject, newLayer);
		}
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
		
		bodyWidth = bodySprite.bounds.size.x;
		bodyHeight = bodySprite.bounds.size.y;
		handSensor.SetPlayerRef(this);
		obstSensorL.SetPlayerRef(this);
		obstSensorR.SetPlayerRef(this);

		rigidbody.mass = 0.2f;
		
		// Set initial values
		SetBoxHolding (null);
		springTouching = null;
		matchingPlatformTouching = null;
	}
	private void IdentifyComponentsRecursively(Transform t) {
		if (t.name == "Body") bodyGO = t.gameObject;
		else if (t.name == "BodySprite") bodySprite = t.gameObject.GetComponent<SpriteRenderer>();
		else if (t.name == "Head") headSprite = t.gameObject.GetComponent<SpriteRenderer>();
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
		// Pass downwards through platforms
		if (matchingPlatformTouching!=null && (Input.GetAxis("Vertical")<-0.8f || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))) {
			PassDownThroughPlatform(matchingPlatformTouching);
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
			// SPRING!
			if (IsTouchingSpring) {
				springTouching.LaunchRigidbody(MyRigidbody, JUMP_FORCE);
			}
			// REGULAR jump!
			else {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, -JUMP_FORCE);
			}
		}
	}
	void PassDownThroughPlatform(Platform platform) {
		EveryMyColliderIgnoreCollision(transform, platform.MyBoxCollider, true);
	}

	void InputLogicGrabbingBoxes() {
		// NOT holding a box...
		if (boxHolding == null) {
			// NEXT to a box and just hit GRAB key?!
			if (handSensor.BoxTouching!=null && Input.GetKey(KEYCODE_BOX_GRAB)) {
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
	/** If the box we're holding escapes our grip, relinquish it! */
	public void OnBoxHoldingExitHandSensor() {
		SetBoxHolding(null);
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

		matchingPlatformTouching = null; // I'll say otherwise in the OnTriggerStay2D function.
	}

	
	// ================================
	//	Physics-themed Methods
	// ================================
	void ApplyGravity() {
		//if (!isGrounded) {
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y+WorldProperties.GRAVITY_FORCE);
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


	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Platform") {
			Platform platform = other.gameObject.GetComponent<Platform>();
			if (platform.ColorID==0 || platform.ColorID==colorID) {
				matchingPlatformTouching = platform;
			}
		}
	}


	void EveryMyColliderIgnoreCollision(Transform t, Collider2D colliderToIgnore, bool doIgnore) {
		// Ignore all colliders on this gameObject
		Collider2D[] myColliders = t.gameObject.GetComponentsInChildren<Collider2D>();
		foreach (Collider2D collider in myColliders) {
			Physics2D.IgnoreCollision(collider, colliderToIgnore, doIgnore);
		}
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








