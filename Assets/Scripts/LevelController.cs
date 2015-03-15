using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	// Constants
	private const string TAG_ENTRANCE = "Entrance";
	private const string TAG_GOAL = "Goal";
	private const string TAG_PLAYER = "Player";
	// Level Object References
	private Player player;
	private GameObject entrance;
	private GameObject goal;

	void Start () {
		ResetLevel ();
	}

	void ResetLevel() {
		// Find level objects!
		entrance = GameObject.FindGameObjectWithTag (TAG_ENTRANCE);
		goal = GameObject.FindGameObjectWithTag (TAG_GOAL);

		// Create player!
		ResetPlayer ();
	}
	void ResetPlayer() {
		GameObject playerGO = GameObject.FindGameObjectWithTag (TAG_PLAYER);
		// Player ALREADY exists...
		if (playerGO != null) {
			player = playerGO.GetComponent<Player>();
		}
		// Player DOESN'T yet exist...!
		else {
			// Load prefab and make player!
			GameObject playerPrefab = (GameObject) Resources.Load ("Player");
			player = ((GameObject)GameObject.Instantiate (playerPrefab)).GetComponent<Player>();
		}
		// Reset player!
		player.Reset ();
		// Move player to entrance!
		player.transform.localPosition = new Vector2(entrance.transform.position.x, entrance.transform.position.y);
	}
	
	void Update () {
	
	}
}
