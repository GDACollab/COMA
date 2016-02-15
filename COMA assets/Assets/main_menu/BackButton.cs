using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.GetComponent<SpriteRenderer> ().enabled == true) {
			if(Input.GetKeyDown(KeyCode.Return)) {
				this.transform.parent.GetComponent<TitleInput>().prevMenu();
			}
		}
	}
}
