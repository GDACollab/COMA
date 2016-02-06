using UnityEngine;
using System.Collections;

public class movScript : MonoBehaviour {

	const float speed = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		BoxCollider2D boxC = this.gameObject.GetComponent<BoxCollider2D> ();
		this.gameObject.GetComponent<Rigidbody2D> ().velocity = 
			new Vector2 (Input.GetAxis ("Horizontal") * speed, Input.GetAxis ("Vertical") * speed);

		
	}


}
