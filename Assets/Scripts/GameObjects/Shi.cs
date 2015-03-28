using UnityEngine;
using System.Collections;

public class Shi : MonoBehaviour {
	// References (external)
	ShiGate myShiGateRef; // the dude who matches my channel! (NOTE: if we want MULTIPLE shiGates to pair with shis, we'll have to make this into an array)
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
	public void SetMyShiGateRef(ShiGate _myShiGateRef) { myShiGateRef = _myShiGateRef; }

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
		isOn = numContactsTouchingMe>0;
		// Update image!
		Debug.Log(isOn);
		spriteOn.enabled = isOn;
		spriteOff.enabled = !isOn;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		numContactsTouchingMe ++;
		UpdateIsOn();
		myShiGateRef.OnShiNumContactsChanged();
	}
	void OnTriggerExit2D(Collider2D other) {
		numContactsTouchingMe --;
		UpdateIsOn();
		myShiGateRef.OnShiNumContactsChanged();
	}
}
