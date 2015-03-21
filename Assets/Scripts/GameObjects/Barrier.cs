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
		bodySprite = GetComponentInChildren<SpriteRenderer>();

		SetColorID(colorID);
	}


	void SetColorID(int newColorID) {
		colorID = newColorID;
		SetLayerRecursively(gameObject, colorID);
		bodySprite.renderer.material.color = Colors.GetLayerColor(colorID);
	}
	private void SetLayerRecursively(GameObject go, int newLayer) {
		go.layer = newLayer;
		foreach (Transform childTransform in go.transform) {
			SetLayerRecursively(childTransform.gameObject, newLayer);
		}
	}

//	void Update() {
//		bodySprite.renderer.material.color = Colors.GetLayerColor(colorID);
//	}
}
