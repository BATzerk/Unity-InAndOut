using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	// Constants
	private const string TAG_ENTRANCE = "Entrance";
	private const string TAG_GOAL = "Goal";
	private const string TAG_MAIN_CAMERA = "MainCamera";
	private const string TAG_PLAYER = "Player";
	// Level Object References
	private GameCamera gameCamera;
	private Player player;
	private GameObject entrance;
	private GameObject goal;

	void Start () {
	}

	public void ResetLevel() {
		// Find level objects!
		gameCamera = GameObject.FindGameObjectWithTag (TAG_MAIN_CAMERA).GetComponent<GameCamera>();
		entrance = GameObject.FindGameObjectWithTag (TAG_ENTRANCE);
		goal = GameObject.FindGameObjectWithTag (TAG_GOAL);

		// Ignore proper physics layers!
		for (int i=1; i<WorldProperties.NUM_COLORS; i++) {
			Physics2D.IgnoreLayerCollision(WorldProperties.BarrierLayer(i), WorldProperties.RigidbodyLayer(i), true);
		}

		// ShiGates!
		ConnectAllShiGatesWithShis();

		// Reset things!
		ResetPlayer ();
		gameCamera.Reset (player.transform);

		// Set initial player colorID!
		player.SetColorID(0);

		// Go ahead and color everything in Environment
		GameObject environmentGO = GameObject.Find("Environment");
		foreach (Transform t in environmentGO.transform) {
			SpriteRenderer spriteRenderer = t.gameObject.GetComponent<SpriteRenderer>();
			if (spriteRenderer != null) {
				spriteRenderer.color = Colors.GetLayerColor(0);
			}
		}
	}

	void ConnectAllShiGatesWithShis() {
		// For every ShiGate in the level, have it find its shis!
		GameObject[] allShiGOs = GameObject.FindGameObjectsWithTag("Shi");
		GameObject[] allShiGateGOs = GameObject.FindGameObjectsWithTag("ShiGate");
		foreach (GameObject shiGateGO in allShiGateGOs) {
			shiGateGO.GetComponent<ShiGate>().Initialize();
			shiGateGO.GetComponent<ShiGate>().FindMyShis(allShiGOs);
		}
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







