using UnityEngine;
using System.Collections;

public class ShiGate : MonoBehaviour {
	// References (external)
	Shi[] myShis;
	// References (internal)
	SpriteRenderer bodySprite;
	BoxCollider2D myCollider;
	// Properties
	[SerializeField]
	private int myChannel;
	private bool areAllShisOn; // updated in OnShiNumContactsChanged

	void Start () {
		// Associate references
		myCollider = GetComponent<BoxCollider2D>();
		bodySprite = GetComponentInChildren<SpriteRenderer>();
	}

	public void FindMyShis(GameObject[] allShiGOs) {
		// Loop through all the shis in the level, and those with the same channel as I will be part of my gang!
		ArrayList myShisArrayList = new ArrayList();
		foreach (GameObject shiGO in allShiGOs) {
			Shi thisShi = shiGO.GetComponent<Shi>();
			if (thisShi!=null && thisShi.MyChannel==myChannel) {
				myShisArrayList.Add(thisShi);
			}
		}
		myShis = new Shi[myShisArrayList.Count];
		for (int i=0; i<myShis.Length; i++) {
			myShis[i] = myShisArrayList[i] as Shi;
			myShis[i].SetMyShiGateRef(this);
		}
	}

	public void OnShiNumContactsChanged() {
		areAllShisOn = true; // I'll say otherwise in a moment.
		// Go ahead and loop through every shi. If all are on, then sweet! I'm on!
		foreach (Shi tempShi in myShis) {
			if (!tempShi.IsOn) {
				areAllShisOn = false;
			}
		}
		// Update my contact enabled-ness!
		if (areAllShisOn) {
			myCollider.enabled = false;
			bodySprite.color = new Color(1,1,1, 0.4f);
		}
		else {
			myCollider.enabled = true;
			bodySprite.color = Color.white;
		}
	}


}
