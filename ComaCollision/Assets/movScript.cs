using UnityEngine;
using System.Collections;

public class movScript : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		this.gameObject.GetComponent<Rigidbody2D> ().velocity = 
			new Vector2 (Input.GetAxisRaw ("Horizontal") * speed, Input.GetAxisRaw ("Vertical") * speed);

		
	}


}
