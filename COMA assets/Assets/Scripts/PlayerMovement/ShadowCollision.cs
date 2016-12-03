using UnityEngine;
using System.Collections;

public class ShadowCollision : MonoBehaviour {

	//set a boolean to change playerrs sprite sheet to be shaded version

	void OnTriggerEnter2D(Collider2D obj){
		if (obj.tag == "Player")
			obj.gameObject.GetComponent<PlayerMovement> ().shaded = true;
	}

	void OnTriggerExit2D(Collider2D obj){
		if (obj.tag == "Player")
			obj.gameObject.GetComponent<PlayerMovement> ().shaded = false;
	}
}
