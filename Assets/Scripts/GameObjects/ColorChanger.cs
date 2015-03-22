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
		if (other.tag == tagToConvert) {
			// HACK TEMPORARY TODO: Like, have some Colorable component or something? For things that can be colored?
			Box box = other.GetComponent<Box>();
			Player player = other.GetComponent<Player>();
			if (box != null) box.SetColorID(colorID);
			if (player != null) player.SetColorID(colorID);
		}
	}

}
