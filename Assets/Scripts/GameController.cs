using UnityEditor;
using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	// References (external)
	[SerializeField]
	LevelController levelController;
	// Game Properties
	private int currentLevelNum;

	void Start () {
		// Associate references
		if (levelController == null) { levelController = GameObject.Find("LevelController").GetComponent<LevelController>(); }

		// Set currentLevelNum by scene name!
		// HACK!!! Cut out just a lot of the string's prefixes by HARDCODED amount. If we change folder structures or anything, this gets messed up!!
		currentLevelNum = int.Parse(EditorApplication.currentScene.Substring(19, 1));

		// Reset level!
		levelController.ResetLevel ();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.F1)) { LoadLevel(1); }
		else if (Input.GetKeyDown(KeyCode.F2)) { LoadLevel(2); }
		else if (Input.GetKeyDown(KeyCode.F3)) { LoadLevel(3); }
		else if (Input.GetKeyDown(KeyCode.F4)) { LoadLevel(4); }
	}

	
	public void LoadPreviousLevel() {
		LoadLevel(Mathf.Max(0, currentLevelNum-1));
	}
	public void LoadNextLevel() {
		LoadLevel((currentLevelNum+1));
	}
	private void LoadLevel(int levelNum) { Application.LoadLevel("Level" + levelNum); }
}
