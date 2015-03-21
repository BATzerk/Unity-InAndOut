using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour {
	// References
	SpriteRenderer spriteRenderer;
	// Properties
	[SerializeField]
	private int colorID;
	[SerializeField]
	private string tagToConvert;
	
	void Start () {
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		// Set ColorID ONCE, now!
		gameObject.layer = colorID;
		spriteRenderer.renderer.material.color = Colors.GetLayerColor(colorID);
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
