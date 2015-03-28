using UnityEngine;
using System.Collections;

public class PlayerHandSensor : MonoBehaviour {
	// References
	private Player playerRef;
	// Properties
	private Box boxTouching;
	// Getters
	public Box BoxTouching {
		get { return boxTouching; }
	}
	public void SetPlayerRef(Player player) { playerRef = player; }


	void Start () {
		boxTouching = null;
	}


	void OnTriggerEnter2D(Collider2D other) {
		// Just touched a BOX?!
		if (other.tag == "Box") {
			// This is the box I am now touching, yo!
			SetBoxTouching(other.GetComponent<Box>());
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		// Just left a BOX?!
		if (other.tag == "Box") {
			Box thisBox = other.GetComponent<Box>();
			// Was this the box I was just touching?!
			if (boxTouching == thisBox) {
				// Nullify boxTouching!
				SetBoxTouching(null);
			}
			// Was this the box the player was HOLDING?!?
			if (playerRef.BoxHolding == thisBox) {
				// Make player let go of this box, man! It's too far from our hands!
				playerRef.OnBoxHoldingExitHandSensor();
			}
		}
	}



	void SetBoxTouching(Box tempBox) {
		// FIRST, if I'm already touching a box, tell it it's no longer selected!
		if (boxTouching != null) {
			boxTouching.OnUngrabbable ();
		}

		// Set new boxTouching!
		boxTouching = tempBox;

		// Notify boxTouching that it's grabbable!
		if (boxTouching != null) {
			boxTouching.OnGrabbable ();
		}
	}
}



