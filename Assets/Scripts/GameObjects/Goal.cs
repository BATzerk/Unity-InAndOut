using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	// References (external)
	private GameController gameController;
	// References (internal)
	private SpriteRenderer bodySprite;
	// Properties
	[SerializeField]
	private string sceneDestination; // The name of the scene file we will go to.
	bool isPlayerTouchingMe;

	void Start () {
		// Associate references
		bodySprite = GetComponentInChildren<SpriteRenderer>();
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	void Update () {
		if (isPlayerTouchingMe) {
			bodySprite.color = Color.green;
			// UP ARROW to advance to next level!
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				gameController.LoadLevel(sceneDestination);
			}
		}
		else {
			bodySprite.color = Color.gray;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			isPlayerTouchingMe = true;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			isPlayerTouchingMe = false;
		}
	}
}
