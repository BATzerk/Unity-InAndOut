using UnityEngine;
using System.Collections;

public class ShiGateShiIcon : MonoBehaviour {
	// References (internal)
	SpriteRenderer spriteOn;
	SpriteRenderer spriteOff;
	
	public void Initialize () {
		// Associate references
		IdentifyComponentsRecursively(transform);
	}
	private void IdentifyComponentsRecursively(Transform t) {
		if (t.name == "SpriteOn") spriteOn = t.GetComponent<SpriteRenderer>();
		else if (t.name == "SpriteOff") spriteOff = t.GetComponent<SpriteRenderer>();
		// Do it again recursively!
		foreach (Transform childTransform in t) { IdentifyComponentsRecursively(childTransform); }
	}
	
	public void UpdateIsOn(bool isOn) {
		// Update image!
		spriteOn.enabled = isOn;
		spriteOff.enabled = !isOn;
	}
}
