using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shi : MonoBehaviour {
	// References (external)
	List<ShiGate> myShiGateRefs; // the dude(s) who match(es) my channel!
	// References (internal)
	SpriteRenderer spriteOn;
	SpriteRenderer spriteOff;
	BoxCollider2D myCollider;
	// Properties
	[SerializeField]
	private int myChannel;
	private int numContactsTouchingMe;
	private bool isOn;
	// Getters
	public bool IsOn { get { return isOn; } }
	public int MyChannel { get { return myChannel; } }
	public void AddShiGateRef(ShiGate shiGate) {
		if (myShiGateRefs == null) { myShiGateRefs = new List<ShiGate>(); }
		myShiGateRefs.Add(shiGate);
	}

	void Start () {
		// Associate references
		myCollider = GetComponent<BoxCollider2D>();
		IdentifyComponentsRecursively(transform);

		// Reset variables
		numContactsTouchingMe = 0;
		UpdateIsOn();
	}
	private void IdentifyComponentsRecursively(Transform t) {
		if (t.name == "SpriteOn") spriteOn = t.GetComponent<SpriteRenderer>();
		else if (t.name == "SpriteOff") spriteOff = t.GetComponent<SpriteRenderer>();
		// Do it again recursively!
		foreach (Transform childTransform in t) { IdentifyComponentsRecursively(childTransform); }
	}

	private void UpdateIsOn() {
		// Update isOn
		isOn = numContactsTouchingMe > 0;
		// Update image!
		spriteOn.enabled = isOn;
		spriteOff.enabled = !isOn;
	}

	private void OnNumContactsTouchingMeChanged() {
		UpdateIsOn();
		foreach (ShiGate shiGate in myShiGateRefs) {
			shiGate.OnShiNumContactsChanged();
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.isTrigger) { return; } // Solid objects only.
		numContactsTouchingMe ++;
		OnNumContactsTouchingMeChanged();
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.isTrigger) { return; } // Solid objects only.
		numContactsTouchingMe --;
		OnNumContactsTouchingMeChanged();
	}

}
