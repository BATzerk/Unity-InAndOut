using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {
	// References (internal)
	private SpriteRenderer spriteNeutral; // just the base spring image, with nothing going on.
	private SpriteRenderer spriteLit; // the spring when it's lit up! When the player is over me.
	// Properties
	[SerializeField]
	public float Strength; // the SCALE of how much velocity the player will have added for his/her jump! So 1 would be no affect, 2 would be twice the normal jump, and 0.5 would be an ironic mini-hop.
	bool isPlayerTouchingMe;

	void Start () {
		// Associate references
		IdentifyComponentsRecursively(transform);
	}
	private void IdentifyComponentsRecursively(Transform t) {
		if (t.name == "SpriteLit") spriteLit = t.GetComponent<SpriteRenderer>();
		else if (t.name == "SpriteNeutral") spriteNeutral = t.GetComponent<SpriteRenderer>();
		// Do it again recursively!
		foreach (Transform childTransform in t) {
			IdentifyComponentsRecursively(childTransform);
		}
	}
	
	void Update () {
		if (isPlayerTouchingMe) {
			spriteLit.color = Color.Lerp(spriteLit.color,Color.white, Time.deltaTime*10);
		}
		else {
			spriteLit.color = Color.Lerp(spriteLit.color,Color.clear, Time.deltaTime*10);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			isPlayerTouchingMe = true;
			Player player = other.GetComponent<Player>();
			player.SpringTouching = this;
		}
//		else if (other.tag == "Box") {
//			is = true;
//			Player player = other.GetComponent<Player>();
//			player.SpringTouching = this;
//		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			isPlayerTouchingMe = false;
			Player player = other.GetComponent<Player>();
			player.SpringTouching = null;
		}
	}
}
