using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {
	// ================================
	//	Properties
	// ================================
	// Constants
	private const float GAP_TO_PLAYER = 16f;
	private const float BASE_LAUNCH_FORCE = -600f; // Ideally, this SHOULD MATCH player's JUMP_FORCE. So boxes go the same height the player does.
	// References (external)
	private Player myPlayerRef; // which player is grabbing me. Passed in on onGrabbing.
	private Spring springTouching; // the spring I'm touching, set by the spring, not me. It knows me, too. If I'm released while touching a spring, I'll get launched!
	private ColorChanger colorChangerTouching; // the colorChanger I'm touching, set by it, not me. If I'm released while touching a ColorChanger, I'll change to that color!
	// References (internal)
	private SpriteRenderer spriteRenderer;
	private Rigidbody2D myRigidbody;
	GameObject obstructionSensorsGO; // the whole GameObject containing my two obstruction sensors. For rotating independently of me so the sensors actually stay on my left/right sides.
	BoxObstructionSensor obstSensorL; // Left obstruction sensor
	BoxObstructionSensor obstSensorR; // Right obstruction sensor
	SpriteRenderer obstructionDebugSpriteL;
	SpriteRenderer obstructionDebugSpriteR;
	// Properties
	private bool isGrabbable; // theoretically grabbable if the player hits SHIFT key
	private float bodyWidth; // how wide this box is! Currently based on my sprite's size :P
	private float holdingOffsetX; // the distance between me and myPlayerRef when I'm being held.
	[SerializeField]
	private int colorID;
	public int ColorID { get { return colorID; } }

	// Getters (private)
	// Getters (public)
	public bool IsBeingHeld { get { return myPlayerRef != null; } } // actually being held/grabbed/dragged/sensually caressed right now
	public Rigidbody2D MyRigidbody { get { return myRigidbody; } }
	public bool IsObstructionL { get { return obstSensorL.IsObstruction; } }
	public bool IsObstructionR { get { return obstSensorR.IsObstruction; } }
	public Spring SpringTouching { get { return springTouching; } set { springTouching = value; } }
	public ColorChanger ColorChangerTouching { get { return colorChangerTouching; } set { colorChangerTouching = value; } }
	
	
	public void SetColorID(int newColorID) {
		colorID = newColorID;
		SetLayerRecursively(this.gameObject, WorldProperties.RigidbodyLayer(colorID));
		spriteRenderer.renderer.material.color = Colors.GetLayerColor(colorID);
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
		// Identify components
		myRigidbody = GetComponent<Rigidbody2D> ();
		IdentifyComponentsRecursively(transform);
		
		obstSensorL.SetBoxRef(this);
		obstSensorR.SetBoxRef(this);

		// DEBUG/TEMPORARY stuff
		bodyWidth = spriteRenderer.bounds.size.x;

		// Set initial values
		isGrabbable = false;
		myPlayerRef = null;
		springTouching = null;
		colorChangerTouching = null;
		
		SetColorID(colorID);

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
		// Set my velocity to the player's current velocity!
		myRigidbody.velocity = new Vector2(myPlayerRef.MyRigidbody.velocity.x*0.4f, myRigidbody.velocity.y);
		// Nullify myPlayerRef
		myPlayerRef = null;
		UpdateVisualsBasedOnGrabVariables ();
		// Touching a spring? Launch me, dawg!
		if (springTouching != null) {
			float launchForce = BASE_LAUNCH_FORCE * springTouching.Strength;
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, myRigidbody.velocity.y - launchForce);
		}
		// Touching a ColorChanger? Change my color, Al!
		if (colorChangerTouching != null) {
			SetColorID(colorChangerTouching.ColorID);
		}
	}
	
	
	
	
	// ================================
	//	Fixed Update
	// ================================
	void FixedUpdate() {
		// Apply gravity
		myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y+WorldProperties.GRAVITY_FORCE);
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
//		float playerRot = Mathf.Deg2Rad * myPlayerRef.MyRigidbody.rotation;
		// HACKY/TEMPORARY: this constant on player's velocity was totally eyeballed. Might be COMPLETELY off.
//		float targetPosX = (myPlayerRef.transform.position.x+Mathf.Cos (playerRot)*holdingOffsetX);
		float targetPosX = (myPlayerRef.MyRigidbody.position.x+holdingOffsetX);
		targetPosX += myPlayerRef.MyRigidbody.velocity.x/60;
//		float targetPosY = myPlayerRef.transform.position.y;// + Mathf.Sin (playerRot)*holdingOffsetX;
		float targetPosY = this.transform.position.y;
		myRigidbody.position = new Vector2(targetPosX, targetPosY);
//		myRigidbody.velocity = new Vector2(myPlayerRef.MyRigidbody.velocity.x, myPlayerRef.MyRigidbody.velocity.y);
	}

}




