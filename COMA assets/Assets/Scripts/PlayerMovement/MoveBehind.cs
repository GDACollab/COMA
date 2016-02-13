using UnityEngine;
using System.Collections;

public class MoveBehind : MonoBehaviour {

	public float bluePillarDistance = -2.0f;

	void OnTriggerStay2D(Collider2D obj){
		if (obj.tag == "Player" && obj.transform.position.y <= transform.position.y)
			obj.transform.GetComponent<SpriteRenderer> ().sortingOrder = 9;

		if (obj.tag == "Player" && obj.transform.position.y < transform.position.y + bluePillarDistance)
			obj.transform.GetComponent<SpriteRenderer> ().sortingOrder = 20;
	}

	void OnTriggerExit2D(Collider2D obj){
		if (obj.tag == "Player")
			obj.transform.GetComponent<SpriteRenderer> ().sortingOrder = 20;
	}
}
