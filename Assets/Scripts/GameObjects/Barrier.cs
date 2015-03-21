using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour {
	// References
	SpriteRenderer spriteRenderer;
	// Properties
	[SerializeField]
	private int colorID = -1;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();

		SetColorID(colorID);
	}

	void SetColorID(int newColorID) {
		colorID = newColorID;
		gameObject.layer = newColorID;
		spriteRenderer.renderer.material.color = Colors.GetLayerColor(colorID);
	}
}
