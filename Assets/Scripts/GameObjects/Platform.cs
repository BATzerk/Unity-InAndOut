using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	// References (internal)
	private BoxCollider2D boxCollider; // the thing that you stand on
	private PlatformUnderTrigger underTrigger; // the trigger UNDER the platform that disables/enables the collision with my main boxCollider.
	private SpriteRenderer bodySprite;
	// Properties
	private float height;

	void Start () {
		IdentifyComponentsRecursively(transform);

		height = bodySprite.bounds.size.y;
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

	
	public void OnUnderTriggerEnter(Collider2D other) {
		// Touch the under-trigger? Disable collisions with the main collider!
		Physics2D.IgnoreCollision(other, boxCollider, true);
	}
	public void OnUnderTriggerExit(Collider2D other) {
		// Stop touching the under-trigger? RE-enable collisions with the main collider!
		Physics2D.IgnoreCollision(other, boxCollider, false);
	}

}
