using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {
	// Properties
//	[SerializeField]
//	float mass = 4;
	// References
	private SpriteRenderer spriteRenderer;

	void Start () {
		// Set mass!
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
//		rigidbody.mass = mass;

		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();

		// I'm not grabbable by default.
		OnUngrabbable ();
	}
	
	public void OnGrabbable() {
		spriteRenderer.renderer.material.color = Color.cyan;
	}
	public void OnUngrabbable() {
		spriteRenderer.renderer.material.color = Color.blue;
	}
}
