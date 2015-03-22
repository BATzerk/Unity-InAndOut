using UnityEngine;
using System.Collections;

public class WorldProperties : MonoBehaviour {
	
	public const int NUM_COLORS = 3; // so we can set layers properly
	public const float GRAVITY_FORCE = -26;
	
	static public int RigidbodyLayer(int colorID) {
		return 8 + colorID;
	}
	static public int BarrierLayer(int colorID) {
		return 8 + colorID + NUM_COLORS;
	}
	/*
	static public int PlayerLayer(int colorID) {
		return 8 + colorID + NUM_COLORS*0;
	}
	static public int BoxLayer(int colorID) {
		return 8 + colorID + NUM_COLORS*1;
	}
	static public int BarrierLayer(int colorID) {
		return 8 + colorID + NUM_COLORS*2;
	}
	*/

}
