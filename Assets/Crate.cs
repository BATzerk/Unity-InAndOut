using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {
	// Properties
	[SerializeField]
	float mass = 4;

	void Start () {
		// Set mass!
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D> ();
		rigidbody.mass = mass;
	}
}
