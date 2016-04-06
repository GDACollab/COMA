using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	public string LevelLoad = "";

	void OnCollisionEnter2D(Collision2D obj){
		if (obj.gameObject.tag == "Player") {
			Application.LoadLevel (LevelLoad);
		}

		if (obj.gameObject.tag == "Pillar") {
			GameObject.Find("Player").GetComponent<SpriteRenderer> ().sortingOrder = 9;
		}
	}

	void OnTriggerExit2D(Collider2D obj){
		if (obj.tag == "Boundry" && obj.name == "BackGroundBlockCeiling")
			GameObject.Find("Player").GetComponent<SpriteRenderer> ().sortingOrder = 100;
	}
}
