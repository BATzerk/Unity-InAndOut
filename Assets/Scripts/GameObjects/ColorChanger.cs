using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class ColorChanger : MonoBehaviour {
	// References (internal)
	SpriteRenderer bodySprite;
	// Properties
	[SerializeField]
	private int colorID;
	[SerializeField]
	private string tagToConvert;

	public int ColorID { get { return colorID; } }
	
	void Start () {
		// DON'T do anything if we're in the editor.
//		if (Application.platform==RuntimePlatform.WindowsEditor || Application.platform==RuntimePlatform.OSXEditor) { return; }

		bodySprite = GetComponentInChildren<SpriteRenderer>();

		// Set ColorID ONCE, now!
		gameObject.layer = colorID;
		bodySprite.renderer.material.color = Colors.GetLayerColor(colorID);
	}

	void Update() {
		// ONLY update this stuff in EDIT mode!
		if (Application.platform==RuntimePlatform.WindowsEditor || Application.platform==RuntimePlatform.OSXEditor) {
			if (bodySprite == null) { bodySprite = GetComponentInChildren<SpriteRenderer>(); }

			Material tempMaterial = new Material(bodySprite.renderer.sharedMaterial);
			tempMaterial.color = Colors.GetLayerColor(colorID);
			bodySprite.renderer.sharedMaterial = tempMaterial;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Just touched the right stuff?
		// DEBUG HACK TEMPORARY allow both player AND box.
		if (other.tag == "Player" || other.tag=="Box") {
//		if (other.tag == tagToConvert) {
			// HACK TEMPORARY TODO: Like, have some Colorable component or something? For things that can be colored?
			// Box
			Box box = other.GetComponent<Box>();
			if (box != null) {
				box.ColorChangerTouching = this;
				// If the box ISN'T being grabbed, change its color now!
				if (!box.IsBeingHeld) {
					box.SetColorID(colorID);
				}
			}
			// Player
			Player player = other.GetComponent<Player>();
			if (player != null) player.SetColorID(colorID);
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == tagToConvert) {
			// Box
			Box box = other.GetComponent<Box>();
			if (box != null) {
				// If I'm the ColorChanger this box was touching, then disassociate our relationship!
				if (box.ColorChangerTouching == this) {
					box.ColorChangerTouching = null;
				}
			}
			// Player
			Player player = other.GetComponent<Player>();
		}
	}

}
