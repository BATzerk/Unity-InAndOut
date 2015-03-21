using UnityEngine;
using System.Collections;

public class Colors : MonoBehaviour {

	static public Color GetLayerColor(int colorID) {
		if (colorID == 0) return Color.white;
		if (colorID == 1) return new Color(61/255f,190/255f,255/255f);
		if (colorID == 2) return new Color(255/255f,172/255f,38/255f); //FFAC26
		return Color.white;
	}
}
