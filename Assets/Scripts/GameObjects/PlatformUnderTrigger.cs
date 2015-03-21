using UnityEngine;
using System.Collections;

public class PlatformUnderTrigger : MonoBehaviour {
	// References (external)
	private Platform platform;
	public void SetPlatform(Platform _platform) { platform = _platform; }
	// Properties


	void Start () {
	}
	
	
	void OnTriggerEnter2D(Collider2D other) {
		// Player?
		Debug.Log(other.tag);
		//		if (other.tag == "Player") {
		//			Player player = other.GetComponent<Player>();
		//			// Player BELOW the platform?
		//			if (player.transform.position.y-player.BodyHeight*0.5f < transform.position.y+height*0.5f) {
		//				Physics2D.IgnoreCollision(other, boxCollider);
		//			}
		//			Debug.Log((player.transform.position.y-player.BodyHeight*0.5f) + "  " + (transform.position.y+height*0.5f));
		//		}
		platform.OnUnderTriggerEnter(other);
	}
	
	//	void OnTriggerStay2D(Collider2D other) {
	//		oneWay = true;
	//	}
	
	void OnTriggerExit2D(Collider2D other) {
		platform.OnUnderTriggerExit(other);
	}
}
