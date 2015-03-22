using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {
	// References (internal)
	private SpriteRenderer spriteNeutral; // just the base spring image, with nothing going on.
	private SpriteRenderer spriteLit; // the spring when it's lit up! When the player is over me.
	// References (external)
	Box boxTouchingMe;
	Player playerTouchingMe;
	// Properties
	[SerializeField]
	public float Strength; // the SCALE of how much velocity the player will have added for his/her jump! So 1 would be no affect, 2 would be wayy up high (remember: doubles velocity, not distance), and 0.5 would be an ironic mini-hop.

	void Start () {
		// Associate references
		IdentifyComponentsRecursively(transform);
		// Reset things
		boxTouchingMe = null;
		playerTouchingMe = null;
	}
	private void IdentifyComponentsRecursively(Transform t) {
		if (t.name == "SpriteLit") spriteLit = t.GetComponent<SpriteRenderer>();
		else if (t.name == "SpriteNeutral") spriteNeutral = t.GetComponent<SpriteRenderer>();
		// Do it again recursively!
		foreach (Transform childTransform in t) {
			IdentifyComponentsRecursively(childTransform);
		}
	}



	void Update () {
		// I'll launch a box if it's touching me AND being held.
		bool willLaunchBox = boxTouchingMe!=null && boxTouchingMe.IsBeingHeld;
		// I'll launch a player if it's touching me AND not holding a box.
		bool willLaunchPlayer = playerTouchingMe!=null && !playerTouchingMe.IsHoldingBox;

		if (willLaunchBox || willLaunchPlayer) {
			spriteLit.color = Color.Lerp(spriteLit.color,Color.white, Time.deltaTime*10);
		}
		else {
			spriteLit.color = Color.Lerp(spriteLit.color,Color.clear, Time.deltaTime*10);
		}
	}



	void OnTriggerEnter2D(Collider2D other) {
		// -- Player --
		if (other.tag == "Player") {
			// Set playerTouchingMe!
			playerTouchingMe = other.GetComponent<Player>();
			playerTouchingMe.SpringTouching = this;
		}
		// -- Box --
		else if (other.tag == "Box") {
			boxTouchingMe = other.GetComponent<Box>();
			boxTouchingMe.SpringTouching = this;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		// -- Player --
		if (other.tag == "Player") {
			Player player = other.GetComponent<Player>();
			// If this is the same player that was touching me, then nullify the relationship with playerTouchingMe!
			if (player == playerTouchingMe) {
				playerTouchingMe.SpringTouching = null;
				playerTouchingMe = null;
			}
		}
		// -- Box --
		else if (other.tag == "Box") {
			Box box = other.GetComponent<Box>();
			// If this is the same box that was touching me, then nullify the relationship with boxTouchingMe!
			if (box == boxTouchingMe) {
				boxTouchingMe.SpringTouching = null;
				boxTouchingMe = null;
			}
		}
	}
}





