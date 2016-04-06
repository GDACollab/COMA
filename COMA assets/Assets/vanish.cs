using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class vanish : MonoBehaviour {

	float next = 0;
	float wait = .4f;
	Text words;

	// Use this for initialization
	void Start () {
		words = this.GetComponent<Text> ();
		words.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (words.enabled == true) {
			if (next == 0) {
				next = Time.time + wait;
			} else if (Time.time > next) {
				next = 0;
				words.enabled = false;
			}
		}
	
	}
}
