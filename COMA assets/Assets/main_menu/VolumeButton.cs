using UnityEngine;
using System.Collections;

public class VolumeButton : MonoBehaviour {

	private int curVol;
	private const int maxVol = 100;
	// Use this for initialization
	void Start () {
		curVol = 50;	
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<SpriteRenderer> ().enabled == true) {
			transform.GetComponentInChildren<SpriteRenderer> ().enabled = true;
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (curVol > 0) {
					transform.GetChild (0).transform.localPosition -= new Vector3 (1, 0, 0);
					curVol--;
				}
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				if (curVol < maxVol) {
					transform.GetChild (0).transform.localPosition += new Vector3 (1, 0, 0);
					curVol++;
				}
			}
		} else {
			transform.GetComponentInChildren<SpriteRenderer> ().enabled = false;
		}
	}
}
