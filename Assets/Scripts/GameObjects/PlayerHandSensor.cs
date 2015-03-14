using UnityEngine;
using System.Collections;

public class PlayerHandSensor : MonoBehaviour {
	// Properties
	private Crate crateTouching;
	// Getters
	public Crate CrateTouching {
		get { return crateTouching; }
	}


	void Start () {
		crateTouching = null;
	}


	void OnTriggerEnter2D(Collider2D other) {
		// Just touched a CRATE?!
		if (other.tag == "Crate") {
			// This is the crate I am now touching, yo!
			SetCrateTouching(other.GetComponent<Crate>());
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		// Just left a CRATE?!
		if (other.tag == "Crate") {
			// Was this the crate I was just touching?!
			if (crateTouching == other.GetComponent<Crate>()) {
				// Nullify crateTouching!
				SetCrateTouching(null);
			}
		}
	}



	void SetCrateTouching(Crate tempCrate) {
		// FIRST, if I'm already touching a crate, tell it it's no longer selected!
		if (crateTouching != null) {
			crateTouching.OnUngrabbable ();
		}

		// Set new crateTouching!
		crateTouching = tempCrate;

		// Notify crateTouching that it's grabbable!
		if (crateTouching != null) {
			crateTouching.OnGrabbable ();
		}
	}
}



