using UnityEngine;
using System.Collections;

public class movScript : MonoBehaviour {

<<<<<<< HEAD
	public float speed = 5.0f;
=======
	public float speed;
>>>>>>> origin/master

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		this.gameObject.GetComponent<Rigidbody2D> ().velocity = 
<<<<<<< HEAD
			new Vector2 (Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw ("Vertical") * speed);
=======
			new Vector2 (Input.GetAxisRaw ("Horizontal") * speed, Input.GetAxisRaw ("Vertical") * speed);
>>>>>>> origin/master

		
	}


}
