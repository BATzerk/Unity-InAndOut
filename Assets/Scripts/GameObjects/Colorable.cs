using UnityEngine;
using System.Collections;

public class Colorable : MonoBehaviour {
	// Properties
	private int colorID = -1;
	public int ColorID { get { return colorID; } }

	void Setup() {
		SetColorID(1);
	}

	void SetColorID(int newColorID) {
		colorID = newColorID;
		gameObject.layer = 7 + newColorID;
	}
}
