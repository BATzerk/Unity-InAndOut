using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour {
	// References
	SpriteRenderer spriteRenderer;
	// Properties
	[SerializeField]
	private int colorID;
	
	void Start () {
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		// Set ColorID ONCE, now!
		gameObject.layer = colorID;
		spriteRenderer.renderer.material.color = Colors.GetLayerColor(colorID);
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Just touched a BOX?!
		if (other.tag == "Box") {
			other.GetComponent<Box>().SetColorID(colorID);
		}
	}

}
