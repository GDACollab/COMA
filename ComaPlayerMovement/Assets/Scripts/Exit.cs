using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	public string LevelLoad = "";

	void OnCollisionEnter2D(Collision2D obj){
		if (obj.gameObject.tag == "Player") {
			Application.LoadLevel (LevelLoad);
		}
	}
}
