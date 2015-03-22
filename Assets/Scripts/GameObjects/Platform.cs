using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	// References (internal)
	private BoxCollider2D boxCollider; // the thing that you stand on
	private PlatformUnderTrigger underTrigger; // the trigger UNDER the platform that disables/enables the collision with my main boxCollider.
	private SpriteRenderer bodySprite;
	// Properties
	private float height;
	private int numPlayerCollidersTouching; // SUPER HACK and not scalable. The player has MULTIPLE colliders. We want to trigger them ALL on/off, but ONLY when the first and last ones touch me.

	void Start () {
		IdentifyComponentsRecursively(transform);

		height = bodySprite.bounds.size.y;
		numPlayerCollidersTouching = 0;

		underTrigger.SetPlatform(this);
	}
	private void IdentifyComponentsRecursively(Transform t) {
		if (t.name == "BodySprite") bodySprite = t.GetComponent<SpriteRenderer>();
		else if (t.name == "Collider") boxCollider = t.GetComponent<BoxCollider2D>();
		else if (t.name == "UnderTrigger") underTrigger = t.GetComponent<PlatformUnderTrigger>();
		// Do it again recursively!
		foreach (Transform childTransform in t) {
			IdentifyComponentsRecursively(childTransform);
		}
	}

	
	/*
	public void OnUnderTriggerEnter(Collider2D other) {
		// Touch the under-trigger?
		// Disable collisions with the main collider!
		Physics2D.IgnoreCollision(other, boxCollider, true);
	}
	public void OnUnderTriggerExit(Collider2D other) {
		// Stop touching the under-trigger?
		// RE-enable collisions with the main collider!
		Physics2D.IgnoreCollision(other, boxCollider, false);
	}
	*/

	public void OnUnderTriggerEnter(Collider2D other) {
		// Touch the under-trigger?
		// -- PLAYER --
		if (other.tag == "Player") {
			numPlayerCollidersTouching ++;
			Debug.Log("Enter  " + numPlayerCollidersTouching);
			if (numPlayerCollidersTouching != 1) { return; }
			// Disable collisions with ALL colliders of this object!
			Collider2D[] colliders = other.gameObject.GetComponents<Collider2D>();
			foreach (Collider2D tempCollider in colliders) {
				Physics2D.IgnoreCollision(boxCollider, tempCollider, true);
			}
		}
		// -- NOT PLAYER --
		else {
			// Disable collisions with the main collider!
			Physics2D.IgnoreCollision(other, boxCollider, true);
		}
	}
	public void OnUnderTriggerExit(Collider2D other) {
		// Stop touching the under-trigger?
		// -- PLAYER --
		if (other.tag == "Player") {
			numPlayerCollidersTouching --;
			Debug.Log("xxxxit  " + numPlayerCollidersTouching);
			if (numPlayerCollidersTouching != 0) { return; }
			// RE-enable collisions with ALL colliders of this object!
			Collider2D[] colliders = other.gameObject.GetComponents<Collider2D>();
			foreach (Collider2D tempCollider in colliders) {
				Physics2D.IgnoreCollision(boxCollider, tempCollider, false);
			}
		}
		// -- NOT PLAYER --
		else {
			// RE-enable collisions with the main collider!
			Physics2D.IgnoreCollision(other, boxCollider, false);
		}
	}

}
