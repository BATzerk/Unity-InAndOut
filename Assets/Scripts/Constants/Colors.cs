using UnityEngine;
using System.Collections;

public class Colors : MonoBehaviour {
	
	static public Color GetLayerColor(int colorID) {
		if (colorID == 0) return new Color(85/225f,98/225f,112/225f);
		if (colorID == 1) return new Color(78/255f,205/255f,196/255f);
		if (colorID == 2) return new Color(200/255f,244/255f,100/255f);
		if (colorID == 3) return new Color(255/255f,107/255f,107/255f);
		if (colorID == 4) return new Color(255/255f,36/255f,101/255f);
		if (colorID == 3) return new Color(175/255f,255/255f,36/255f);
		//		if (colorID == 1) return new Color(61/255f,190/255f,255/255f);
		//		if (colorID == 2) return new Color(255/255f,172/255f,38/255f); //FFAC26
		//		if (colorID == 3) return new Color(228/255f,74/255f,255/255f); //E44AFF
		//		if (colorID == 4) return new Color(255/255f,36/255f,101/255f); //E44AFF
		//		if (colorID == 3) return new Color(175/255f,255/255f,36/255f); //E44AFF
		return Color.white;
	}
	
	static public Color GetShiColor(int channel) {
		if (channel == 0) return new Color(228/255f,74/255f,255/255f);
		if (channel == 1) return new Color(134/255f,74/255f,255/255f);
		if (channel == 2) return new Color(48/225f,90/225f,255/225f);
		if (channel == 3) return new Color(48/255f,255/255f,255/255f);
		if (channel == 4) return new Color(191/255f,179/255f,90/255f);
		if (channel == 5) return new Color(175/255f,255/255f,36/255f);
		return Color.white;
	}
}
