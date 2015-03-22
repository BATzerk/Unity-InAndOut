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
		for (int i=0; i<WorldProperties.NUM_COLORS; i++) {
			Debug.Log (WorldProperties.BarrierLayer(i) + "  " + WorldProperties.BoxLayer(i));
			Physics2D.IgnoreLayerCollision(WorldProperties.BarrierLayer(i), WorldProperties.BoxLayer(i), true);
			Physics2D.IgnoreLayerCollision(WorldProperties.BarrierLayer(i), WorldProperties.PlayerLayer(i), true);
		}
//		Physics2D.IgnoreLayerCollision(15, 16);
		/*
		Physics2D.IgnoreLayerCollision(1,1);
		Physics2D.IgnoreLayerCollision(2,2);
		Physics2D.IgnoreLayerCollision(3,3);
		Physics2D.IgnoreLayerCollision(4,4);
		Physics2D.IgnoreLayerCollision(5,5);
		Physics2D.IgnoreLayerCollision(6,6);
		Physics2D.IgnoreLayerCollision(7,7);
		Physics2D.IgnoreLayerCollision(8,8);
		Physics2D.IgnoreLayerCollision(9,9);
		Physics2D.IgnoreLayerCollision(10,10);
		*/

		// Reset things!
		ResetPlayer ();
		gameCamera.Reset (player.transform);

		// Set initial player colorID!
		player.SetColorID(0);
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







