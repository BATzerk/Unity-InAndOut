using UnityEngine;
using System.Collections;

public class PlayerHandSensor : MonoBehaviour {
	// Properties
	private Box boxTouching;
	// Getters
	public Box BoxTouching {
		get { return boxTouching; }
	}


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
			// Was this the box I was just touching?!
			if (boxTouching == other.GetComponent<Box>()) {
				// Nullify boxTouching!
				SetBoxTouching(null);
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



