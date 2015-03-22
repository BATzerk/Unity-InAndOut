using UnityEngine;
using System.Collections;

public class Colors : MonoBehaviour {

	static public Color GetLayerColor(int colorID) {
		if (colorID == 0) return Color.white;
		if (colorID == 1) return new Color(61/255f,190/255f,255/255f);
		if (colorID == 2) return new Color(255/255f,172/255f,38/255f); //FFAC26
		if (colorID == 3) return new Color(228/255f,74/255f,255/255f); //E44AFF
		if (colorID == 4) return new Color(255/255f,36/255f,101/255f); //E44AFF
		if (colorID == 3) return new Color(175/255f,255/255f,36/255f); //E44AFF
		return Color.white;
	}
}
