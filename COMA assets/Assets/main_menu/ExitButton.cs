using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.GetComponent<SpriteRenderer> ().enabled == true) {
			if(Input.GetKeyDown(KeyCode.Return)) {
				Application.Quit();
				//Debug.Log("Hit");
			}
		}
	}
}
