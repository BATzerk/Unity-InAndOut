using UnityEngine;
using System.Collections;

public class ShiGateShiIcon : MonoBehaviour {
	// References (internal)
	SpriteRenderer spriteOn;
	SpriteRenderer spriteOff;
	
	public void Initialize (int myChannel) {
		// Associate references
		IdentifyComponentsRecursively(transform);
		// Set color of spriteOn!
		spriteOff.color = new Color(1,1,1, 0.4f);// Colors.GetShiColor(myChannel);
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
