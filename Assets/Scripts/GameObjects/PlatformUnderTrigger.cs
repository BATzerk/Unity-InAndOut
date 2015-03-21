using UnityEngine;
using System.Collections;

public class PlatformUnderTrigger : MonoBehaviour {
	// References (external)
	private Platform platform;
	public void SetPlatform(Platform _platform) { platform = _platform; }

	
	void OnTriggerEnter2D(Collider2D other) {
		platform.OnUnderTriggerEnter(other);
	}
	void OnTriggerExit2D(Collider2D other) {
		platform.OnUnderTriggerExit(other);
	}
}
