using UnityEngine;
using System.Collections;

public class ShiGate : MonoBehaviour {
	// References (external)
	Shi[] myShis;
	// References (internal)
	SpriteRenderer bodySprite;
	BoxCollider2D myCollider;
	// Components
	ShiGateShiIcon[] shiIcons; // these are made dynamically, when I find my shis.
	// Properties
	[SerializeField]
	private int myChannel;
	private int numShisOn; // updated in OnShiNumContactsChanged
	private bool AreAllShisOn { get { return numShisOn >= myShis.Length; } }

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
		// Make myShis and shiIcons!
		GameObject shiIconPrefab = (GameObject) Resources.Load("LevelElements/ShiGateShiIcon");
		myShis = new Shi[myShisArrayList.Count];
		shiIcons = new ShiGateShiIcon[myShisArrayList.Count];
		for (int i=0; i<myShis.Length; i++) {
			myShis[i] = myShisArrayList[i] as Shi;
			myShis[i].AddShiGateRef(this);
			// shiIcon!
			shiIcons[i] = ((GameObject) Instantiate(shiIconPrefab)).GetComponent<ShiGateShiIcon>();
			shiIcons[i].Initialize(); // EESH. For whatever reason, Start doesn't happen immediately after it's instantiated. So call an initialize function to get it ready for updating right away.
			shiIcons[i].transform.parent = this.transform;
			// Position it!
			float shiIconPosY = (i+0.5f-myShis.Length*0.5f) * bodySprite.bounds.size.y*0.7f;
			shiIcons[i].transform.localPosition = new Vector2(0, shiIconPosY);
		}
		UpdateShiIcons();
	}

	private void UpdateShiIcons() {
		for (int i=0; i<shiIcons.Length; i++) { shiIcons[i].UpdateIsOn(i < numShisOn); }
	}

	public void OnShiNumContactsChanged() {
		numShisOn = 0; // I'll increment this in a hot sec.
		// Go ahead and loop through every shi. If all are on, then sweet! I'm on!
		foreach (Shi tempShi in myShis) {
			if (tempShi.IsOn) {
				numShisOn ++;
			}
		}
		// Update my contact enabled-ness!
		if (AreAllShisOn) {
			myCollider.enabled = false;
			bodySprite.color = new Color(1,1,1, 0.2f);
		}
		else {
			myCollider.enabled = true;
			bodySprite.color = Color.white;
		}
		UpdateShiIcons();
	}


}



