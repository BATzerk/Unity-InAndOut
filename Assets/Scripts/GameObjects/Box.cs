using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {
	// Constants
	private const float GAP_TO_PLAYER = 10f;
	// References
	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rigidbody;
	private Player myPlayerRef; // which player is grabbing me. Passed in on onGrabbing.
	// Properties
	private bool isGrabbable; // theoretically grabbable if the player hits SHIFT key
	private float bodyWidth; // how wide this crate is! Currently based on my sprite's size :P
	private float holdingOffsetX; // the distance between me and myPlayerRef when I'm being held.

	// Getters
	private bool IsBeingHeld { get { return myPlayerRef != null; } } // actually being held/grabbed/dragged/sensually caressed right now
	public Rigidbody2D MyRigidbody { get { return rigidbody; } }


	void Start () {
		// Set mass!
		rigidbody = GetComponent<Rigidbody2D> ();
//		rigidbody.mass = mass;

		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();

		// DEBUG/TEMPORARY stuff
		bodyWidth = spriteRenderer.bounds.size.x;

		// Set initial values
		isGrabbable = false;
		myPlayerRef = null;

		SetColorBasedOnGrabVariables ();
	}

	private void SetColorBasedOnGrabVariables() {
		if (IsBeingHeld) spriteRenderer.renderer.material.color = Color.yellow;
		else if (isGrabbable) spriteRenderer.renderer.material.color = Color.cyan;
		else spriteRenderer.renderer.material.color = Color.blue;
	}
	
	public void OnGrabbable() {
		isGrabbable = true;
		SetColorBasedOnGrabVariables ();
	}
	public void OnUngrabbable() {
		isGrabbable = false;
		SetColorBasedOnGrabVariables ();
	}
	
	public void OnGrabbed(Player thisPlayer) {
		myPlayerRef = thisPlayer;
		SetColorBasedOnGrabVariables ();
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
		SetColorBasedOnGrabVariables ();
	}



	void FixedUpdate() {
		CrateHoldingMath ();
	}
	
	void CrateHoldingMath() {
		if (myPlayerRef == null) { return; }
		// Set my position! Note: If the player is at an angle, then we'll not just be using an x offset but both x and y.
		float playerRot = Mathf.Deg2Rad * myPlayerRef.MyRigidbody.rotation;
		// HACKY/TEMPORARY: this constant on player's velocity was totally eyeballed. Might be COMPLETELY off.
		float targetPosX = (myPlayerRef.transform.position.x+Mathf.Cos (playerRot)*holdingOffsetX) + myPlayerRef.MyRigidbody.velocity.x/60;
//		float targetPosX = (myPlayerRef.MyRigidbody.position.x+holdingOffsetX) + myPlayerRef.MyRigidbody.velocity.x/60;
		float targetPosY = myPlayerRef.transform.position.y;// + Mathf.Sin (playerRot)*holdingOffsetX;
		rigidbody.position = new Vector2(targetPosX, targetPosY);
//		rigidbody.velocity = new Vector2(myPlayerRef.MyRigidbody.velocity.x, myPlayerRef.MyRigidbody.velocity.y);
	}
}




