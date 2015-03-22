using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class Barrier : MonoBehaviour {
	// References
	SpriteRenderer bodySprite;
	// Properties
	[SerializeField]
	private int colorID = -1;


	void Start () {
		// DON'T do anything if we're in the editor.
//		if (Application.platform==RuntimePlatform.WindowsEditor || Application.platform==RuntimePlatform.OSXEditor) { return; }

		bodySprite = GetComponentInChildren<SpriteRenderer>();

		SetColorID(colorID);
	}


	void SetColorID(int newColorID) {
//		if (Application.platform==RuntimePlatform.WindowsEditor || Application.platform==RuntimePlatform.OSXEditor) { return; }
		colorID = newColorID;
		Debug.Log ("Barrier setting color to " + WorldProperties.BarrierLayer(colorID));
		SetLayerRecursively(gameObject, WorldProperties.BarrierLayer(colorID));
		bodySprite.renderer.material.color = Colors.GetLayerColor(colorID);
	}
	private void SetLayerRecursively(GameObject go, int newLayer) {
		go.layer = newLayer;
		foreach (Transform childTransform in go.transform) {
			SetLayerRecursively(childTransform.gameObject, newLayer);
		}
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
		Debug.Log("Trigger  " + this.gameObject.layer + "   " + other.gameObject.tag);
	}
}
