using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {
	// Properties
//	[SerializeField]
//	float mass = 4;
	// References
	private SpriteRenderer spriteRenderer;
	// Properties
	private bool isGrabbing; // actually being grabbed right now
	private bool isGrabbable; // theoretically grabbable if the player hits SHIFT key

	void Start () {
		// Set mass!
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
//		rigidbody.mass = mass;

		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();

		// Set initial values
		isGrabbing = false;
		isGrabbable = false;

		SetColorBasedOnGrabVariables ();
	}

	private void SetColorBasedOnGrabVariables() {
		if (isGrabbing) spriteRenderer.renderer.material.color = Color.yellow;
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
	
	public void OnGrabbing() {
		isGrabbing = true;
		SetColorBasedOnGrabVariables ();
	}
	public void OnUngrabbing() {
		isGrabbing = false;
		SetColorBasedOnGrabVariables ();
	}
}




