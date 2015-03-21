using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {
	// ================================
	//	Properties
	// ================================
	// Constants
	private const float GAP_TO_PLAYER = 10f;
	// References (internal)
	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rigidbody;
	GameObject obstructionSensorsGO; // the whole GameObject containing my two obstruction sensors. For rotating independently of me so the sensors actually stay on my left/right sides.
	BoxObstructionSensor obstSensorL; // Left obstruction sensor
	BoxObstructionSensor obstSensorR; // Right obstruction sensor
	SpriteRenderer obstructionDebugSpriteL;
	SpriteRenderer obstructionDebugSpriteR;
	// References (external)
	private Player myPlayerRef; // which player is grabbing me. Passed in on onGrabbing.
	// Properties
	private bool isGrabbable; // theoretically grabbable if the player hits SHIFT key
	private float bodyWidth; // how wide this box is! Currently based on my sprite's size :P
	private float holdingOffsetX; // the distance between me and myPlayerRef when I'm being held.
	private int colorID = -1;
	public int ColorID { get { return colorID; } }

	// Getters (private)
	private bool IsBeingHeld { get { return myPlayerRef != null; } } // actually being held/grabbed/dragged/sensually caressed right now
	// Getters (public)
	public Rigidbody2D MyRigidbody { get { return rigidbody; } }
	public bool IsObstructionL { get { return obstSensorL.IsObstruction; } }
	public bool IsObstructionR { get { return obstSensorR.IsObstruction; } }
	
	
	public void SetColorID(int newColorID) {
		colorID = newColorID;
		gameObject.layer = newColorID;
		spriteRenderer.renderer.material.color = Colors.GetLayerColor(colorID);
	}


	
	
	// ================================
	//	Start
	// ================================
	void Start () {
		// Identify components
		rigidbody = GetComponent<Rigidbody2D> ();
		IdentifyComponentsRecursively(transform);
		
		obstSensorL.SetBoxRef(this);
		obstSensorR.SetBoxRef(this);

		// DEBUG/TEMPORARY stuff
		bodyWidth = spriteRenderer.bounds.size.x;

		// Set initial values
		isGrabbable = false;
		myPlayerRef = null;
		
		SetColorID(2);

		UpdateVisualsBasedOnGrabVariables ();
	}
	private void IdentifyComponentsRecursively(Transform t) {
		if (t.name == "BodySprite") spriteRenderer = t.GetComponent<SpriteRenderer>();
		else if (t.name == "ObstructionSensors") obstructionSensorsGO = t.gameObject;
		else if (t.name == "ObstructionSensorL") obstSensorL = t.GetComponent<BoxObstructionSensor>();
		else if (t.name == "ObstructionSensorR") obstSensorR = t.GetComponent<BoxObstructionSensor>();
		else if (t.name == "ObstructionDebugSpriteL") obstructionDebugSpriteL = t.GetComponent<SpriteRenderer>();
		else if (t.name == "ObstructionDebugSpriteR") obstructionDebugSpriteR = t.GetComponent<SpriteRenderer>();
		// Do it again recursively!
		foreach (Transform childTransform in t) {
			IdentifyComponentsRecursively(childTransform);
		}
	}

	
	
	// ================================
	//	Updating Visuals
	// ================================
	private void UpdateVisualsBasedOnGrabVariables() {
//		if (IsBeingHeld) spriteRenderer.renderer.material.color = Color.yellow;
//		else if (isGrabbable) spriteRenderer.renderer.material.color = Color.cyan;
//		else spriteRenderer.renderer.material.color = Color.blue;
	}
	
	
	// ================================
	//	Grabbing Events
	// ================================
	public void OnGrabbable() {
		isGrabbable = true;
		UpdateVisualsBasedOnGrabVariables ();
	}
	public void OnUngrabbable() {
		isGrabbable = false;
		UpdateVisualsBasedOnGrabVariables ();
	}
	
	public void OnGrabbed(Player thisPlayer) {
		myPlayerRef = thisPlayer;
		UpdateVisualsBasedOnGrabVariables ();
		// Determine holdingOffsetX!
		if (transform.position.x < myPlayerRef.transform.position.x) {
			holdingOffsetX = -(myPlayerRef.BodyWidth + this.bodyWidth + GAP_TO_PLAYER) * 0.5f;
		}
		else {
			holdingOffsetX = (myPlayerRef.BodyWidth + this.bodyWidth + GAP_TO_PLAYER) * 0.5f;
		}
	}
	public void OnUngrabbed() {
		myPlayerRef = null;
		UpdateVisualsBasedOnGrabVariables ();
	}
	
	
	
	
	// ================================
	//	Fixed Update
	// ================================
	void FixedUpdate() {
		BoxHoldingMath ();

		if (IsObstructionL) obstructionDebugSpriteL.color = Color.red;
		else obstructionDebugSpriteL.color = Color.gray;
		if (IsObstructionR) obstructionDebugSpriteR.color = Color.red;
		else obstructionDebugSpriteR.color = Color.gray;

		// Rotate my left/right obstruction sensors SELECTIVELY! So, like, once we pass 45 degrees, then ROTATE them so they're not above/below me. Etc.
		obstructionSensorsGO.transform.localEulerAngles = new Vector3(
			obstructionSensorsGO.transform.localEulerAngles.x,
			obstructionSensorsGO.transform.localEulerAngles.y,
			-Mathf.Round(this.transform.localEulerAngles.z/90f)*90f
		);
	}
	
	
	
	// ================================
	//	Holding Math
	// ================================
	void BoxHoldingMath() {
		if (myPlayerRef == null) { return; }
		// Set my position! Note: If the player is at an angle, then we'll not just be using an x offset but both x and y.
		float playerRot = Mathf.Deg2Rad * myPlayerRef.MyRigidbody.rotation;
		// HACKY/TEMPORARY: this constant on player's velocity was totally eyeballed. Might be COMPLETELY off.
//		float targetPosX = (myPlayerRef.transform.position.x+Mathf.Cos (playerRot)*holdingOffsetX);
		float targetPosX = (myPlayerRef.MyRigidbody.position.x+holdingOffsetX);
		targetPosX += myPlayerRef.MyRigidbody.velocity.x/60;
//		float targetPosY = myPlayerRef.transform.position.y;// + Mathf.Sin (playerRot)*holdingOffsetX;
		float targetPosY = this.transform.position.y;
		rigidbody.position = new Vector2(targetPosX, targetPosY);
//		rigidbody.velocity = new Vector2(myPlayerRef.MyRigidbody.velocity.x, myPlayerRef.MyRigidbody.velocity.y);
	}

}




